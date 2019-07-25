using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2
{
    class SquareModel
    {
        /// <summary>
        /// 背景色
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// 列番号
        /// </summary>
        public int ColumnIndex { get; set; }
        /// <summary>
        /// 行番号
        /// </summary>
        public int RowIndex { get; set; }

    }
}
