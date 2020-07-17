using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace Lab8_Exercise
{
    [PluginAttribute("Lab8_Add", "TwentyTwo", DisplayName = "Lab8_Exec_Add", ToolTip = "A tutorial project by TwentyTwo")]

    public class MainClass : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            // current document (.NET)
            Document doc = Application.ActiveDocument;
            // current document (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            // current selected items
            ModelItemCollection items = doc.CurrentSelection.SelectedItems;

            if (items.Count > 0)
            {
                // input dialog
                InputDialog dialog = new InputDialog();
                dialog.ShowDialog();
                foreach (ModelItem item in items)
                {
                    // convert ModelItem to COM Path
                    InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(item);
                    // Get item's PropertyCategoryCollection
                    InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                    // create a new Category (PropertyDataCollection)
                    InwOaPropertyVec newcate = (InwOaPropertyVec)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                    // create a new Property (PropertyData)
                    InwOaProperty newprop = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                    // set PropertyName
                    newprop.name = dialog.PropertyName+"_InternalName";
                    // set PropertyDisplayName
                    newprop.UserName = dialog.PropertyName;
                    // set PropertyValue
                    newprop.value = dialog.PropertyValue;
                    // add PropertyData to Category
                    newcate.Properties().Add(newprop);
                    // add CategoryData to item's CategoryDataCollection
                    cpropcates.SetUserDefined(0, dialog.CategoryName, dialog.CategoryName+"_InternalName", newcate);
                }
            }
            return 0;
        }

    }
}
