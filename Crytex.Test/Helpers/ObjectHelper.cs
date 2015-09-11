namespace Crytex.Test
{
    public static class ObjectHelper
    {
        public static object GetValueProperty(this object obj, string nameProperty)
        {
            return obj.GetType().GetProperty(nameProperty).GetValue(obj);
        }
    }
}