namespace GMABot.Timers
{
    // Custom timer, we need it so that we can keep track of the time that
    // the message should be executed
    class MessageTimer : System.Timers.Timer
    {
        public TimeOnly Time { get; set; }

        public MessageTimer(double interval, TimeOnly time) : base(interval)
        {
            this.Time = time;
        }
    }

}
