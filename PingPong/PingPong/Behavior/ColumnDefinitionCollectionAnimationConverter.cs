using Xamarin.Forms;

namespace PingPong.Behavior
{
    public class ColumnDefinitionCollectionAnimationConverter : IAnimationValueConverter
    {
        public object GetValue(double value, object StartValue, object EndValue)
        {
            var newCollection = new ColumnDefinitionCollection();

            var index = 0;
            foreach (var collumnDefinition in (ColumnDefinitionCollection)EndValue)
            {
                if (index < (StartValue as ColumnDefinitionCollection).Count)
                {
                    var oldWidth = (StartValue as ColumnDefinitionCollection)[index].Width.Value;
                    var newWidth = collumnDefinition.Width.Value;

                    newCollection.Add(new ColumnDefinition() { Width = new GridLength(oldWidth + value * (newWidth - oldWidth), type: collumnDefinition.Width.GridUnitType) });
                    index++;
                }
                else
                {
                    newCollection.Add(new ColumnDefinition() { Width = new GridLength(collumnDefinition.Width.Value, type: collumnDefinition.Width.GridUnitType) });
                }
            }

            return newCollection;
        }

        public bool IsValueEqual(object first, object second)
        {
            var firstCollection = first as ColumnDefinitionCollection;
            var secondtCollection = second as ColumnDefinitionCollection;

            bool isEqual = firstCollection.Count == secondtCollection.Count;

            if (isEqual)
            {
                for (int i = 0; i < firstCollection.Count && isEqual; i++)
                {
                    isEqual = firstCollection[i].Width.Value == secondtCollection[i].Width.Value
                        && firstCollection[i].Width.GridUnitType == secondtCollection[i].Width.GridUnitType;
                }
            }

            return isEqual;
        }
    }
}
