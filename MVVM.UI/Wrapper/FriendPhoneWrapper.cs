using MVVM.Model;

namespace MVVM.UI.Wrapper
{
    public class FriendPhoneWrapper : ModelWrapper<FriendPhoneNumber>
    {

        public FriendPhoneWrapper(FriendPhoneNumber model) : base(model)
        {

        }
        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
