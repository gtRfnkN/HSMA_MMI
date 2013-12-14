using System.Windows;
using System.Windows.Controls;

namespace CityGuide.Extensions
{
    public static class UIElementCollectionExtentions
    {
        #region Contains Methods
        public static bool Contains(this UIElementCollection collection, UIElement element)
        {
            bool found = false;

            for (int counter = 0; counter < collection.Count; counter++)
            {
                if (collection[counter].Uid.Equals(element.Uid))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public static bool Contains(this UIElementCollection collection, UIElement element, out int index)
        {
            bool found = false;
            index = -1;

            for (int counter = 0; counter < collection.Count; counter++)
            {
                if (collection[counter].Uid.Equals(element.Uid))
                {
                    found = true;
                    index = counter;
                    break;
                }
            }
            return found;
        }
        #endregion
    }
}
