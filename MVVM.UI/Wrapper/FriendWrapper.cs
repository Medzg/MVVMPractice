using MVVM.Model;
using MVVM.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Wrapper
{
   public class FriendWrapper : ViewModelBase,INotifyDataErrorInfo
    {
        public FriendWrapper(Friend model)
        {
            Model = model;
        }

        public Friend Model { get; }

        public int Id { get {return Model.Id;} }


        public string FirstName { get { return Model.FirstName; }
            set {
                Model.FirstName = value;OnPropertyChanged();
                ValidateProperty(nameof(FirstName));
            }
        }

        private void ValidateProperty(string propertyName)
        {
            this.DeleteError(propertyName);
            switch(propertyName){
                case nameof(FirstName): if (string.Equals(FirstName, "Trash", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName, "Trash are always trash");
                    }
                    break;

            }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set
            {
                Model.LastName = value; OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return Model.Email; }
            set
            {
                Model.Email = value; OnPropertyChanged();
            }
        }
        private Dictionary<string, List<string>> _errorByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors =>_errorByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorByPropertyName.ContainsKey(propertyName) ? _errorByPropertyName[propertyName] : null;
        }
        private void OnErrorChanged(string propertyName)
        {

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName,string error)
        {
            if (!_errorByPropertyName.ContainsKey(propertyName))
            {

                _errorByPropertyName[propertyName] = new List<string>();
            } if(_errorByPropertyName.ContainsKey(propertyName))
            {
                _errorByPropertyName[propertyName].Add(error);
                OnErrorChanged(propertyName);

            }

        }
        private void DeleteError(string propertyName)
        {
            if (_errorByPropertyName.ContainsKey(propertyName))
            {
                _errorByPropertyName.Remove(propertyName);
                OnErrorChanged(propertyName);

            }
        }
    }
}
