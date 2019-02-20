namespace ECS.Test.Unit
{
    internal class TempSensor : ITempSensor
    {
        public int Temp { get; set; }

        public TempSensor()
        {
            Temp = 0;
        }

        public int GetTemp()
        {
            return Temp;
        }
    }

    internal class Heater : IHeater
    {
        public int TurnOffCalledTimes { get; set; }
        public int TurnOnCalledTimes { get; set; }

        public Heater()
        {
            TurnOffCalledTimes = 0;
            TurnOnCalledTimes = 0;
        }

        public void TurnOn()
        {
            ++TurnOnCalledTimes;
        }



        public void TurnOff()
        {
            ++TurnOffCalledTimes;
        }
    }


    internal class Window : IWindow
    {
        public int OpenCalledTimes { get; set; }
        public int CloseCalledTimes { get; set; }

        public Window()
        {
            OpenCalledTimes = 0;
            CloseCalledTimes = 0;
        }

        public void Close()
        {
            ++CloseCalledTimes;
        }

        public void Open()
        {
            ++OpenCalledTimes;
        }
    }

}
