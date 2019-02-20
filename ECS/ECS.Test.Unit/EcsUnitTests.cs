using System;
using NUnit.Framework;


namespace ECS.Test.Unit
{
    [TestFixture]
    public class EcsUnitTests
    {
        // member variables to hold uut and fakes
        private FakeTempSensor _fakeTempSensor;
        private FakeHeater _fakeHeater;
        private ECS _uut;
        private FakeWindow _fakeWindow;

        [SetUp]
        public void Setup()
        {
            // Create the fake stubs and mocks
            _fakeHeater = new FakeHeater();
            _fakeTempSensor = new FakeTempSensor();
            _fakeWindow = new FakeWindow();
            // Inject them into the uut via the constructor
            _uut = new ECS(_fakeTempSensor, _fakeHeater, _fakeWindow, 25, 28);
        }

        #region Threshold tests

        [Test]
        public void Thresholds_ValidUpperTemperatureThresholdSet_NoExceptionsThrown()
        {
            // Check that it doesn't throw
            // First parameter is a lambda expression, implicitly acting
            Assert.That(() => { _uut.UpperTemperatureThreshold = 27; }, Throws.Nothing);
        }

        [Test]
        public void Thresholds_ValidLowerTemperatureThresholdSet_NoExceptionsThrown()
        {
            // Check that it doesn't throw 
            // First parameter is a lambda expression, implicitly acting
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
        public void Regulate_TempIsLow_HeaterIsTurnedOn()
        {
            // Setup stub with desired response
            _fakeTempSensor.Temp = 20;
            // Act
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            Assert.That(_fakeHeater.TurnOnCalledTimes, Is.EqualTo(1));
        }


        [Test]
        public void Regulate_TempIsLow_WindowIsClosed()
        {
            // Setup stub with desired response
            _fakeTempSensor.Temp = 20;
            // Act
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region T == Tlow

        [Test]
        public void Regulate_TempIsAtLowerThreshold_HeaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 25;
            // Act
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            Assert.That(_fakeHeater.TurnOffCalledTimes, Is.EqualTo(1));
        }

        [Test]
        public void Regulate_TempIsAtLowerThreshold_WindowIsClosed()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 25;
            // Act
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region Tlow < T < Thigh

        [Test]
        public void Regulate_TempIsBetweenLowerAndUpperThresholds_HeaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            Assert.That(_fakeHeater.TurnOnCalledTimes, Is.EqualTo(0));
        }

        [Test]
        public void Regulate_TempIsBetweenLowerAndUpperThresholds_WindowIsClosed()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region T == Thigh

        [Test]
        public void Regulate_TempIsAtUpperThreshold_HeaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            Assert.That(_fakeHeater.TurnOffCalledTimes, Is.EqualTo(1));
        }

        [Test]
        public void Regulate_TempIsAtUpperThreshold_WindowIsClosed()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            Assert.That(_fakeWindow.CloseCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #region T > Thigh

        [Test]
        public void Regulate_TempIsAboveUpperThreshold_HeaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 27;
            _uut.Regulate();

            // Assert on the mock - was the heater called correctly
            Assert.That(_fakeHeater.TurnOffCalledTimes, Is.EqualTo(1));
        }

        [Test]
        public void Regulate_TempIsAboveUpperThreshold_WindowIsOpened()
        {
            // Setup the stub with desired response
            _fakeTempSensor.Temp = 29;
            _uut.Regulate();

            // Assert on the mock - was the window called correctly
            Assert.That(_fakeWindow.OpenCalledTimes, Is.EqualTo(1));
        }

        #endregion

        #endregion
    }
}
