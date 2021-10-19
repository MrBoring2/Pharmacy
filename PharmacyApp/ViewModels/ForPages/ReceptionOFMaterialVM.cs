using Aspose.BarCode.Generation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.ViewModels.ForPages
{
    public class ReceptionOFMaterialVM : BasePageVM
    {
        public byte[] Image { get; set; }
        public ReceptionOFMaterialVM()
        {
            PageName = "Принять биоматериал";
            NetBarcode.Barcode barcode = new NetBarcode.Barcode("4820000000000", NetBarcode.Type.EAN13, true, 250, 120, NetBarcode.LabelPosition.BottomCenter, NetBarcode.AlignmentPosition.Center, Color.White, Color.Black, new Font("Courier New", 12)); 

            byte[] bytes = barcode.GetByteArray();

            ImageConverter conv = new ImageConverter();

            Image = bytes;
        }
    }
}
