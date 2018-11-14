using System;
using System.Collections.Generic;
using System.Text;

namespace App1780
{
    public class SampleViewModel
    {
        #region singleton
        public static SampleViewModel Instance => _instance ?? (_instance = new SampleViewModel());
        static SampleViewModel _instance;
        SampleViewModel()
        {
            ListItems.Add("Item 1");
            ListItems.Add("This is the second item");
            ListItems.Add("3rd Item <3");
        }
        #endregion

        #region fields
        IList<string> _listItems = new List<string>();
        #endregion

        #region properties
        public IList<string> ListItems
        {
            get { return _listItems; }
            set { _listItems = value; }
        }
        #endregion
    }
}
