using HouseOffice.DAL.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace HouseOffice.WPF.Helpers
{
    public class UserRequestUpdater
    {
        private readonly ObservableCollection<UserRequest> _userRequests;

        public UserRequestUpdater(ObservableCollection<UserRequest> userRequests)
        {
            _userRequests = userRequests;
        }

        public virtual void UpdateRequests(List<UserRequest> userRequests)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _userRequests.Clear();
                foreach (var request in userRequests)
                {
                    _userRequests.Add(request);
                }
            });
        }
    }
}
