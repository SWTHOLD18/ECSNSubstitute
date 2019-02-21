using System;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;


namespace ECS.Test.Unit
{
    [TestFixture]
    public class EcsUnitTests
    {
        // member variables to hold uut and s
        private ECS _uut;
        private ITempSensor _tempSensor;
        private IHeater _heater;
        private IWindow _window;

        [SetUp]
        public void Setup()
        {
            // Create the  stubs and mocks
            _heater = Substitute.For<IHeater>();
            _tempSensor = Substitute.For<ITempSensor>();
            _window = Substitute.For<IWindow>();
            // Inject them into the uut via the constructor
            _uut = new ECS(_tempSensor, _heater, _window, 25, 28);
        }

        #region Threshold tests

        [Test]
        public void Thresholds_ValidUpperTemperatureThresholdSet_NoExceptionsThrown()
        {
            // Check that it doesn't throw
            // First parameter is a lambda expression, implicitly acting

            // Assert on the mock - was the heater called correctly
            Assert.That(() => { _uut.UpperTemperatureThreshold = 27; }, Throws.Nothing);
        }

        [Test]
        public void Thresholds_ValidLowerTemperatureThresholdSet_NoExceptionsThrown()
        {
            // Check that it doesn't throw 
            // First parameter is a lambda expression, implicitly acting
            /*
             _uut
                .When(x => x.LowerTemperatureThreshold.Returns(26))
                .Do(x => throw );
            */
            Assert.That(() => { _uut.LowerTemperatureThreshold = 26; }, Throws.Nothing);
        }

        [Test]
        public void Thresholds_UpperSetToLower_NoExceptionsThrown()
        {
            // Check that it doesn't throw when they are equal
            // First parameter is a lambda expression, implicitly acting
            Assert.That(() => { _uut.UpperTemperatureThreshold = _uut.LowerTemperatureThreshold; }, Throws.Nothing);
        }

        [Test]
        public void Thresholds_LowerSetToUpper_NoExceptionsThrown()
        {
            // Check that it doesn't throw when they are equal
            // First parameter is a lambda expression, implicitly acting
            Assert.That(() => { _uut.LowerTemperatureThreshold = _uut.UpperTemperatureThreshold; }, Throws.Nothing);
        }


        public void Thresholds_InvalidUpperTemperatureThresholdSet_ArgumentExceptionThrown()
        {
            // Check that it throws when upper is illegal
            // First parameter is a lambda expression, implicitly acting
            Assert.That(() => { _uut.UpperTemperatureThreshold = 24; }, Throws.TypeOf<ArgumentException>());
        }

        public void Thresholds_InvalidLowerTemperatureThresholdSet_ArgumentExceptionThrown()
        {
            // Check that it throws when lower is illegal
            // First parameter is a lambda expression, implicitly acting
            Assert.That(() => { _uut.LowerTemperatureThreshold = 29; }, Throws.TypeOf<ArgumentException>());
        }

        #endregion

        #region Regulation tests

        #region T < Tlow

        [Test]
        public void Regulate_tempIsLow_heaterIsTurnedOn()
        {
            // Setup stub with desired response
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold - 1);
            // Act
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            _heater.Received(1).TurnOn();
        }


        [Test]
        public void Regulate_tempIsLow_windowIsClosed()
        {
            // Setup stub with desired response
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold - 1);
            // Act
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            _window.Received(1).Close();
        }

        #endregion

        #region T == Tlow

        [Test]
        public void Regulate_tempIsAtLowerThreshold_heaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold);
            // Act
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            _heater.Received(1).TurnOff();
        }

        [Test]
        public void Regulate_tempIsAtLowerThreshold_windowIsClosed()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold);
            // Act
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            _window.Received(1).Close();
        }

        #endregion

        #region Tlow < T < Thigh

        [Test]
        public void Regulate_tempIsBetweenLowerAndUpperThresholds_heaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(27);
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            _heater.Received(0).TurnOn();
        }

        [Test]
        public void Regulate_tempIsBetweenLowerAndUpperThresholds_windowIsClosed()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(27);
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            _window.Received(1).Close();
        }

        #endregion

        #region T == Thigh

        [Test]
        public void Regulate_tempIsAtUpperThreshold_heaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(28);
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            _heater.Received(1).TurnOff();
        }

        [Test]
        public void Regulate_tempIsAtUpperThreshold_windowIsClosed()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(28);
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            _window.Received(1).Close();
        }

        #endregion

        #region T > Thigh

        [Test]
        public void Regulate_tempIsAboveUpperThreshold_heaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(_uut.UpperTemperatureThreshold + 1);
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            _heater.Received(1).TurnOff();
        }

        [Test]
        public void Regulate_tempIsAboveUpperThreshold_windowIsOpened()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(_uut.UpperTemperatureThreshold + 1);
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            _window.Received(1).Open();
        }

        #endregion

        #endregion
    }
}
