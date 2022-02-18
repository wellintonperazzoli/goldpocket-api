using System.Reflection;

namespace GoldPocket.Models
{
    public class BaseModel
    {
        public object this[string propertyName]
        {
            get
            {
                Type myType = this.GetType();
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                var v = myPropInfo.GetValue(this, null);
                return v;
            }
            set
            {
                Type myType = this.GetType();               
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }
    }
}