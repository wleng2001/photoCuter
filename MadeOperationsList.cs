using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace photoCuter
{
    using OperationsOnImageList = List<tools.OperationOnImage>;

    public class ListViewItem
    {
        public string operation{ get; set;}
        public string value { get; set; }
    }
    internal class MadeOperationsList
    {
        byte quantityOfOperationOnImage = 3;
        OperationsOnImageList[] operationsOnImages;
        OperationsOnImageList tempOperationsList = new OperationsOnImageList();
        
        public int TempListLength
        {
            get
            {
                return tempOperationsList.Count;
            }
        }
        ListView operationsListView;

        ObservableCollection<ListViewItem> Items = new ObservableCollection<ListViewItem>();

        public OperationsOnImageList this[int photoNumber]
        {
            get { return operationsOnImages[photoNumber]; }
        }
        public MadeOperationsList(byte quantityOfOperationOnImage, ListView operationsListView, int photosQuantity)
        {
            this.quantityOfOperationOnImage = quantityOfOperationOnImage;
            this.operationsListView = operationsListView;
            operationsOnImages = new OperationsOnImageList[photosQuantity];
            for (int i = 0; i < photosQuantity; i++)
            {
                operationsOnImages[i] = new OperationsOnImageList();
            }
            tempOperationsList.Capacity = quantityOfOperationOnImage;
            operationsListView.ItemsSource = Items;
        }

        public void AddTempOpearation(tools.OperationOnImage operation)
        {
            tempOperationsList.Add(operation);

            Items.Add(new ListViewItem()
            {
                operation = operation.OperationType,
                value = operation.StrValue
            });
        }

        public void RemoveTempOperations()
        {
            tempOperationsList.Clear();
        }

        public void ClearTempOperations()
        {
            int itemsOnOperationList = operationsListView.Items.Count;
            for (byte i = 1; i <= tempOperationsList.Count; i++)
            {
                Items.RemoveAt(itemsOnOperationList - i);
            }
            RemoveTempOperations();
        }

        public void ClearOperations(int image)
        {
            Items.Clear();
            tempOperationsList.Clear();
            operationsOnImages[image].Clear();
        }

        public void ClearOperations()
        {
            for(int i = 0; i < operationsOnImages.Length; i++)
            {
                ClearOperations(i);
            }
        }

        public void MigrateToMainImageOperation(int imageNumber)
        {
            if(imageNumber < operationsOnImages.Length)
            {
                for (int j = 0; j < tempOperationsList.Count; j++)
                {
                    operationsOnImages[imageNumber].Add(tempOperationsList[j]);
                }
            }
        }

        public void MigrateToMainImageOperation()
        {
            for (int i = 0; i < operationsOnImages.Length; i++)
            {
                MigrateToMainImageOperation(i);
            }
        }

    }
}
