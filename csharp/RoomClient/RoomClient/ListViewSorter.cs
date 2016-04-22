namespace PokemonBattle.RoomClient
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal class ListViewSorter : IComparer
    {
        private int _columnIndex;
        private SortOrder _order;

        public ListViewSorter(int columnIndex) : this(columnIndex, SortOrder.Ascending)
        {
        }

        public ListViewSorter(int columnIndex, SortOrder order)
        {
            this._columnIndex = columnIndex;
            this._order = order;
        }

        public int Compare(object x, object y)
        {
            string text = (x as ListViewItem).SubItems[this._columnIndex].Text;
            string strB = (y as ListViewItem).SubItems[this._columnIndex].Text;
            int num = string.Compare(text, strB);
            if (this._order == SortOrder.Descending)
            {
                num = -num;
            }
            return num;
        }

        public int ColumnIndex
        {
            get
            {
                return this._columnIndex;
            }
            set
            {
                this._columnIndex = value;
            }
        }

        public SortOrder Order
        {
            get
            {
                return this._order;
            }
            set
            {
                this._order = value;
            }
        }
    }
}

