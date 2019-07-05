namespace MVVM.Model
{
    public  class LookUpItem
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
    }
    public class NullLookUpItem : LookUpItem
    {
        public new int? Id { get; set; }
    }
}
