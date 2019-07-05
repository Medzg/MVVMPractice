using MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) :base(model)
        {
           
        }

      

        public int Id { get { return GetValue<int>(); } }


        public string FirstName { get { return GetValue<string>(); }
            set {
                SetValue(value);
              
            }
        }

      

        public string LastName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public int? FavoriteLangugeId { get {
                 return GetValue<int?>(); } set {
                SetValue(value);
                ; } }


        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Trash", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Trash are always trash";
                    }
                    break;

            }
        }

    }
}
