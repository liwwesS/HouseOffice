namespace HouseOffice.WPF.Helpers
{
    public static class EventMediator
    {
        public static event EventHandler IsDialogOpen;
        public static event EventHandler IsDialogClose;

        public static void OnDialogOpen()
        {
            IsDialogOpen?.Invoke(null, EventArgs.Empty);
        }

        public static void OnDialogClose()
        {
            IsDialogClose?.Invoke(null, EventArgs.Empty);
        }
    }
}
