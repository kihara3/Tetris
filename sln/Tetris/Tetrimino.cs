using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp2
{
    /// <summary>
    /// テトリミノクラス
    /// </summary>
    class Tetrimino
    {
        /// <summary>
        /// 乱数ジェネレータ
        /// </summary>
        static System.Random r = new System.Random();

        /// <summary>
        /// テトリミノの型
        /// </summary>
        public enum TetriminoType
        {
            /// <summary>
            /// I型
            /// </summary>
            I,
            /// <summary>
            /// O型
            /// </summary>
            O,
            /// <summary>
            /// S型
            /// </summary>
            S,
            /// <summary>
            /// Z型
            /// </summary>
            Z,
            /// <summary>
            /// J型
            /// </summary>
            J,
            /// <summary>
            /// L型
            /// </summary>
            L,
            /// <summary>
            /// T型
            /// </summary>
            T
        }

        /// <summary>
        /// 操作
        /// </summary>
        public enum Action
        {
            /// <summary>
            /// 左移動
            /// </summary>
            MoveLeft,
            /// <summary>
            /// 右移動
            /// </summary>
            MoveRight,
            /// <summary>
            /// 下移動
            /// </summary>
            MoveDown,
            /// <summary>
            /// 右回転
            /// </summary>
            RotateRight,
            /// <summary>
            /// 左回転
            /// </summary>
            RotateLeft
        }

        /// <summary>
        /// テトリミノ
        /// </summary>
        public TetriminoType Type { get; set; }

        /// <summary>
        /// オフセット
        /// </summary>
        private Point Offset { get; set; } = new Point(0, 0);

        /// <summary>
        /// 角度（0:正位置、1:右90度回転、2:180度回転、3:左90度回転）
        /// </summary>
        private int Angle { get; set; } = 0;

        /// <summary>
        /// ブロックの座標
        /// </summary>
        public Point[] Points { get; set; }

        /// <summary>
        /// 型の座標
        /// </summary>
        private Point[][] BasePoints { get; set; }

        /// <summary>
        /// I型
        /// □□■□□　□□□□□　□□□□□　□□□□□
        /// □□■□□　□□□□□　□□■□□　□□□□□
        /// □□■□□　□■■■■　□□■□□　■■■■□
        /// □□■□□　□□□□□　□□■□□　□□□□□
        /// □□□□□　□□□□□　□□■□□　□□□□□
        /// 
        /// </summary>
        private Point[][] PointsI = new Point[][] {
            // 正位置
            new Point[]{ new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3)},
            // 右90度
            new Point[]{ new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2)},
            // 180度
            new Point[]{ new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4)},
            // 左90度
            new Point[]{ new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2)},
        };

        /// <summary>
        /// O型
        /// ■■
        /// ■■
        /// 
        /// </summary>
        private Point[][] PointsO = new Point[][] {
            // 正位置
            new Point[]{ new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(1, 1)},
            // 右90度
            new Point[]{ new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(1, 1)},
            // 180度
            new Point[]{ new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(1, 1)},
            // 左90度
            new Point[]{ new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(1, 1)},
        };

        /// <summary>
        /// S型
        /// □■■　□■□　□□□　■□□
        /// ■■□　□■■　□■■　■■□
        /// □□□　□□■　■■□　□■□
        /// 
        /// </summary>
        private Point[][] PointsS = new Point[][] {
            // 正位置
            new Point[]{ new Point(0, 1), new Point(1, 0), new Point(1, 1), new Point(2, 0)},
            // 右90度
            new Point[]{ new Point(1, 0), new Point(1, 1), new Point(2, 1), new Point(2, 2)},
            // 180度
            new Point[]{ new Point(0, 2), new Point(1, 1), new Point(1, 2), new Point(2, 1)},
            // 左90度
            new Point[]{ new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2)},
        };

        /// <summary>
        /// Z型
        /// ■■□　□□■　□□□　□■□
        /// □■■　□■■　■■□　■■□
        /// □□□　□■□　□■■　■□□
        /// 
        /// </summary>
        private Point[][] PointsZ = new Point[][] {
            // 正位置
            new Point[]{ new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(2, 1)},
            // 右90度
            new Point[]{ new Point(1, 1), new Point(1, 2), new Point(2, 0), new Point(2, 1)},
            // 180度
            new Point[]{ new Point(0, 1), new Point(1, 1), new Point(1, 2), new Point(2, 2)},
            // 左90度
            new Point[]{ new Point(0, 1), new Point(0, 2), new Point(1, 0), new Point(1, 1)},
        };

        /// <summary>
        /// J型
        /// ■□□　□■■　□□□　□■□
        /// ■■■　□■□　■■■　□■□
        /// □□□　□■□　□□■　■■□
        /// 
        /// </summary>
        private Point[][] PointsJ = new Point[][] {
            // 正位置
            new Point[]{ new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1)},
            // 右90度
            new Point[]{ new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 0)},
            // 180度
            new Point[]{ new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(2, 2)},
            // 左90度
            new Point[]{ new Point(0, 2), new Point(1, 0), new Point(1, 1), new Point(1, 2)},
        };

        /// <summary>
        /// L型
        /// □□■　□■□　□□□　■■□
        /// ■■■　□■□　■■■　□■□
        /// □□□　□■■　■□□　□■□
        /// 
        /// </summary>
        private Point[][] PointsL = new Point[][] {
            // 正位置
            new Point[]{ new Point(0, 1), new Point(1, 1), new Point(2, 0), new Point(2, 1)},
            // 右90度
            new Point[]{ new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 2)},
            // 180度
            new Point[]{ new Point(0, 1), new Point(0, 2), new Point(1, 1), new Point(2, 1)},
            // 左90度
            new Point[]{ new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(1, 2)},
        };

        /// <summary>
        /// T型
        /// □■□　□■□　□□□　□■□
        /// ■■■　□■■　■■■　■■□
        /// □□□　□■□　□■□　□■□
        /// 
        /// </summary>
        private Point[][] PointsT = new Point[][] {
            // 正位置
            new Point[]{ new Point(0, 1), new Point(1, 0), new Point(1, 1), new Point(2, 1)},
            // 右90度
            new Point[]{ new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 1)},
            // 180度
            new Point[]{ new Point(0, 1), new Point(1, 1), new Point(1, 2), new Point(2, 1)},
            // 左90度
            new Point[]{ new Point(0, 1), new Point(1, 0), new Point(1, 1), new Point(1, 2)},
        };

        /// <summary>
        /// 色
        /// </summary>
        public System.Windows.Media.Color Color
        {
            get
            {
                switch (Type)
                {
                    case TetriminoType.I:
                        return Colors.Red;
                    case TetriminoType.O:
                        return Colors.Yellow;
                    case TetriminoType.S:
                        return Colors.Purple;
                    case TetriminoType.Z:
                        return Colors.Green;
                    case TetriminoType.J:
                        return Colors.Blue;
                    case TetriminoType.L:
                        return Colors.Orange;
                    case TetriminoType.T:
                        return Colors.Aqua;
                    default:
                        return Colors.White;
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Tetrimino()
        {
            //　ランダムに型を決める

            int i = r.Next(7);
            switch (i)
            {
                case 0:
                    Type = TetriminoType.I;
                    BasePoints = PointsI;
                    Offset = new Point(2, -4);
                    break;
                case 1:
                    BasePoints = PointsO;
                    Offset = new Point(4, -2);
                    Type = TetriminoType.O;
                    break;
                case 2:
                    BasePoints = PointsS;
                    Offset = new Point(4, -2);
                    Type = TetriminoType.S;
                    break;
                case 3:
                    BasePoints = PointsZ;
                    Offset = new Point(4, -2);
                    Type = TetriminoType.Z;
                    break;
                case 4:
                    BasePoints = PointsJ;
                    Offset = new Point(4, -2);
                    Type = TetriminoType.J;
                    break;
                case 5:
                    BasePoints = PointsL;
                    Offset = new Point(4, -2);
                    Type = TetriminoType.L;
                    break;
                case 6:
                    BasePoints = PointsT;
                    Offset = new Point(4, -2);
                    Type = TetriminoType.T;
                    break;
            }
            Points = BasePoints[0];
        }

        /// <summary>
        /// オフセット計算した座標を取得
        /// </summary>
        /// <returns></returns>
        public Point[] GetOffsetPoint()
        {
            return Points.Select(v => new Point(v.X + Offset.X, v.Y + Offset.Y)).ToArray();
        }

        /// <summary>
        /// 移動、回転後の座標を返す（移動はしない）
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Point[] GetNextPoints(Action action)
        {
            Point offset = Offset;
            Point[] points = Points;

            switch (action)
            {
                case Action.MoveDown:
                    offset.Y += 1;
                    break;
                case Action.MoveRight:
                    offset.X += 1;
                    break;
                case Action.MoveLeft:
                    offset.X -= 1;
                    break;
                case Action.RotateLeft:
                    points = BasePoints[Angle == 0 ? 3 : Angle - 1];
                    break;
                case Action.RotateRight:
                    points = BasePoints[Angle == 3 ? 0 : Angle + 1];
                    break;
            }

            return points.Select(v => new Point(v.X + offset.X, v.Y + offset.Y)).ToArray();
        }

        /// <summary>
        /// 移動、回転後の座標の差分を返す
        /// 0:ブロックが消える座標、1:ブロックが出現する座標
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Point[][] GetPointDiffs(Action action)
        {
            List<Point> old = new List<Point>(GetOffsetPoint());
            List<Point> next = new List<Point>(GetNextPoints(action));
            List<Point> clone_next = new List<Point>(next);
            List<Point> clone_old = new List<Point>(old);

            clone_old.RemoveAll(next.Contains);
            clone_next.RemoveAll(old.Contains);

            return new Point[][]
            {
                clone_old.ToArray(),
                clone_next.ToArray()
            };
        }

        /// <summary>
        /// 移動する（オフセットをずらす）
        /// </summary>
        /// <param name="action"></param>
        public void Move(Action action)
        {
            Point offset = Offset;
            switch (action)
            {
                case Action.MoveDown:
                    offset.Y += 1;
                    break;
                case Action.MoveRight:
                    offset.X += 1;
                    break;
                case Action.MoveLeft:
                    offset.X -= 1;
                    break;
                case Action.RotateLeft:
                    Angle = Angle == 0 ? 3 : Angle - 1;
                    Points = BasePoints[Angle];
                    break;
                case Action.RotateRight:
                    Angle = Angle == 3 ? 0 : Angle + 1;
                    Points = BasePoints[Angle];
                    break;
            }
            Offset = offset;
        }
    }
}
