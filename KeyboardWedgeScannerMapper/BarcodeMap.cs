using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeyboardWedgeScannerMapper
{
    public class BarcodeMap
    {
        private string _barcode;
        private string _partNumber;

        public string Barcode
        {
            get { return _barcode; }
            set { _barcode = value.Trim(); }
        }

        public string PartNumber
        {
            get { return _partNumber; }
            set { _partNumber = value.Trim(); }
        }

        public BarcodeMap()
        {
            Barcode = "";
            PartNumber = "";
        }

        public BarcodeMap(string barcode, string partNumber)
        {
            Barcode = barcode;
            PartNumber = partNumber;
        }

        public bool IsEmpty()
        {
            return Barcode.Equals("") && PartNumber.Equals("");
        }

        public static List<BarcodeMap> LoadBarcodeMapsFromFile(string path)
        {
            if (!File.Exists(path))
                File.Create(path).Dispose();

            return File.ReadAllLines(path).Select(line => line.Split('\t')).Select(columns => new BarcodeMap
            {
                Barcode = columns[0], PartNumber = columns[1]
            }).ToList();
        }

        public static void SaveBarcodeMapToFile(string path, List<BarcodeMap> barcodeMapList)
        {
            File.WriteAllLines(path, barcodeMapList.Select(barcodeMap => barcodeMap.Barcode + '\t' + barcodeMap.PartNumber).ToArray());
        }

        public override string ToString()
        {
            return "Barcode: " + Barcode + ", PartNumber: " + PartNumber;
        }
    }
}
