using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Tools.NewFolder;

internal static class ReplaceItemextExtansion
{
    public static void ReplaceItem<Item>(this ObservableCollection<Item> col, Func<Item, bool> match, Item newItem)
    {
        var oldItem = col.FirstOrDefault(i => match(i));
        var oldIndex = col.IndexOf(oldItem);
        col[oldIndex] = newItem;
    }
}
