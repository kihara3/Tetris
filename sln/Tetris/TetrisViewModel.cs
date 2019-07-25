using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp2
{
    class TetrisViewModel : INotifyPropertyChanged
    {
        DispatcherTimer dispatcherTimer;

        /// <summary>
        /// コマンド　左移動
        /// </summary>
        public ICommand MoveLeftCommand { get; set; }
        /// <summary>
        /// コマンド　右移動
        /// </summary>
        public ICommand MoveRightCommand { get; set; }
        /// <summary>
        /// コマンド　下移動
        /// </summary>
        public ICommand MoveDownCommand { get; set; }
        /// <summary>
        /// コマンド　右回転
        /// </summary>
        public ICommand TurnRightCommand { get; set; }
        /// <summary>
        /// コマンド　左回転
        /// </summary>
        public ICommand TurnLeftCommand { get; set; }
        /// <summary>
        /// コマンド　ゲームスタート
        /// </summary>
        public ICommand GameStartCommand { get; set; }

        private bool _IsPlaying = false;
        /// <summary>
        /// 遊戯中フラグ
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return _IsPlaying;
            }
            set
            {
                _IsPlaying = value;
                NotifyPropertyChanged(nameof(IsPlaying));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        // プロパティ変更を通知します。
        private void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
        private ObservableCollection<SquareModel> _Squares = new ObservableCollection<SquareModel>();
        /// <summary>
        /// マス目の状態管理
        /// </summary>
        public ObservableCollection<SquareModel> Squares
        {
            get
            {
                return _Squares;
            }
            set
            {
                _Squares = value;
                NotifyPropertyChanged(nameof(Squares));
            }
        }

        private int _Score = 0;
        /// <summary>
        /// 得点
        /// </summary>
        public int Score
        {
            get
            {
                return _Score;
            }
            set
            {
                _Score = value;
                NotifyPropertyChanged(nameof(Score));
            }
        }

        private SquareModel[] _NextTetrimino = new SquareModel[] { };
        /// <summary>
        /// 次のテトリミノ
        /// </summary>
        public SquareModel[] NextTetrimino
        {
            get
            {
                return _NextTetrimino;
            }
            set
            {
                _NextTetrimino = value;
                NotifyPropertyChanged(nameof(NextTetrimino));
            }
        }

        private SquareModel[] modelI = new SquareModel[] {
            new SquareModel { Color = Colors.Red, RowIndex=0,ColumnIndex=1},
            new SquareModel { Color = Colors.Red, RowIndex=1,ColumnIndex=1},
            new SquareModel { Color = Colors.Red, RowIndex=2,ColumnIndex=1},
            new SquareModel { Color = Colors.Red, RowIndex=3,ColumnIndex=1}
        };
        private SquareModel[] modelJ = new SquareModel[] {
            new SquareModel { Color = Colors.Blue, RowIndex=0,ColumnIndex=0},
            new SquareModel { Color = Colors.Blue, RowIndex=1,ColumnIndex=0},
            new SquareModel { Color = Colors.Blue, RowIndex=1,ColumnIndex=1},
            new SquareModel { Color = Colors.Blue, RowIndex=1,ColumnIndex=2}
        };
        private SquareModel[] modelL = new SquareModel[] {
            new SquareModel { Color = Colors.Orange, RowIndex=1,ColumnIndex=0},
            new SquareModel { Color = Colors.Orange, RowIndex=1,ColumnIndex=1},
            new SquareModel { Color = Colors.Orange, RowIndex=0,ColumnIndex=2},
            new SquareModel { Color = Colors.Orange, RowIndex=1,ColumnIndex=2}
        };
        private SquareModel[] modelS = new SquareModel[] {
            new SquareModel { Color = Colors.Purple, RowIndex=1,ColumnIndex=0},
            new SquareModel { Color = Colors.Purple, RowIndex=1,ColumnIndex=1},
            new SquareModel { Color = Colors.Purple, RowIndex=0,ColumnIndex=1},
            new SquareModel { Color = Colors.Purple, RowIndex=0,ColumnIndex=2}
        };
        private SquareModel[] modelZ = new SquareModel[] {
            new SquareModel { Color = Colors.Green, RowIndex=0,ColumnIndex=0},
            new SquareModel { Color = Colors.Green, RowIndex=0,ColumnIndex=1},
            new SquareModel { Color = Colors.Green, RowIndex=1,ColumnIndex=1},
            new SquareModel { Color = Colors.Green, RowIndex=1,ColumnIndex=2}
        };
        private SquareModel[] modelO = new SquareModel[] {
            new SquareModel { Color = Colors.Yellow, RowIndex=0,ColumnIndex=0},
            new SquareModel { Color = Colors.Yellow, RowIndex=0,ColumnIndex=1},
            new SquareModel { Color = Colors.Yellow, RowIndex=1,ColumnIndex=0},
            new SquareModel { Color = Colors.Yellow, RowIndex=1,ColumnIndex=1}
        };
        private SquareModel[] modelT = new SquareModel[] {
            new SquareModel { Color = Colors.Aqua, RowIndex=1,ColumnIndex=0},
            new SquareModel { Color = Colors.Aqua, RowIndex=0,ColumnIndex=1},
            new SquareModel { Color = Colors.Aqua, RowIndex=1,ColumnIndex=1},
            new SquareModel { Color = Colors.Aqua, RowIndex=1,ColumnIndex=2}
        };

        /// <summary>
        /// テトリミノ
        /// </summary>
        private Tetrimino tetrimino = null;

        /// <summary>
        /// 次のテトリミノ
        /// </summary>
        private Tetrimino nexttetrimino = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TetrisViewModel()
        {
            // コマンド定義
            MoveLeftCommand = new MoveCommandImpl() { ExecuteHandler = Move_Left };
            MoveRightCommand = new MoveCommandImpl() { ExecuteHandler = Move_Right };
            MoveDownCommand = new MoveCommandImpl() { ExecuteHandler = Move_Down };
            TurnRightCommand = new MoveCommandImpl() { ExecuteHandler = Turn_Right };
            TurnLeftCommand = new MoveCommandImpl() { ExecuteHandler = Turn_Left };
            GameStartCommand = new GameStartCommandImpl() { ExecuteHandler = GameStart};

        }

        #region イベント

        /// <summary>
        /// ゲームスタート
        /// </summary>
        public void GameStart()
        {
            IsPlaying = true;

            // クリア
            NextTetrimino = new SquareModel[] { };
            Score = 0;
            Squares.Clear();
            NotifyPropertyChanged(nameof(Squares));

            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            // タイマー開始
            dispatcherTimer.Start();

        }
        /// <summary>
        /// 左へ移動
        /// </summary>
        public void Move_Left()
        {
            this.Move(Tetrimino.Action.MoveLeft);
        }

        /// <summary>
        /// 右へ移動
        /// </summary>
        public void Move_Right()
        {
            this.Move(Tetrimino.Action.MoveRight);
        }

        /// <summary>
        /// 下へ移動
        /// </summary>
        public void Move_Down()
        {
            this.Move(Tetrimino.Action.MoveDown);
        }

        /// <summary>
        /// 右へ回転
        /// </summary>
        public void Turn_Right()
        {
            this.Move(Tetrimino.Action.RotateRight);
        }
        /// <summary>
        /// 左へ回転
        /// </summary>
        public void Turn_Left()
        {
            this.Move(Tetrimino.Action.RotateLeft);
        }

        /// <summary>
        /// 自動落下
        /// </summary>
        /// <returns></returns>
        public bool Auto_Down()
        {
            Point[][] diffs;

            // テトリミノがない場合（初回、前のテトリミノが移動できなくなった時）
            if (tetrimino == null)
            {
                tetrimino = nexttetrimino?? new Tetrimino();
                diffs = tetrimino.GetPointDiffs(Tetrimino.Action.MoveDown);

                if (!CanMove(diffs[1]))
                {
                    return false;
                }

                nexttetrimino = new Tetrimino();

                switch (nexttetrimino.Type)
                {
                    case Tetrimino.TetriminoType.I:
                        this.NextTetrimino = this.modelI; 
                        break;
                    case Tetrimino.TetriminoType.J:
                        this.NextTetrimino = this.modelJ;
                        break;
                    case Tetrimino.TetriminoType.L:
                        this.NextTetrimino = this.modelL;
                        break;
                    case Tetrimino.TetriminoType.O:
                        this.NextTetrimino = this.modelO;
                        break;
                    case Tetrimino.TetriminoType.S:
                        this.NextTetrimino = this.modelS;
                        break;
                    case Tetrimino.TetriminoType.T:
                        this.NextTetrimino = this.modelT;
                        break;
                    case Tetrimino.TetriminoType.Z:
                        this.NextTetrimino = this.modelZ;
                        break;
                }

                return true;
            }

            // 差分を取得
            diffs = tetrimino.GetPointDiffs(Tetrimino.Action.MoveDown);

            // 移動不可の場合
            if (!CanMove(diffs[1]))
            {
                tetrimino = null;
                // ラインがそろっているかチェック
                CheckLine();

                // スコアに応じて落下速度を調整
                int span = Score * 20;
                if (span < 1000)
                {
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 - span);
                }
                
                return true;
            }

            //　下へ移動
            tetrimino.Move(Tetrimino.Action.MoveDown);

            // マスの色を変更
            SetColor(diffs);

            return true;
        }

        // タイマー Tick処理
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!Auto_Down())
            {
                // 初期表示位置で移動不可となる場合はゲームオーバー
                dispatcherTimer.Stop();
                System.Windows.MessageBox.Show("Game Over!");
                IsPlaying = false;
            }
        }

        #endregion

        /// <summary>
        /// 指定された座標位置が移動可能かチェックする
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private bool CanMove(Point[] points)
        {

            if (points.Any(v => v.X < 0 || v.X > 9 || v.Y > 19))
            {
                return false;
            }

            foreach (Point p in points)
            {
                if (p.Y < 0) continue;
                if (Squares.Any(v=> v.RowIndex == p.Y && v.ColumnIndex == p.X))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 指定された座標位置の色を変更
        /// </summary>
        /// <param name="diffs"></param>
        private void SetColor(Point[][] diffs)
        {
            foreach (Point p in diffs[0])
            {
                if (p.Y < 0) continue;
                SquareModel model = Squares.FirstOrDefault(v => v.RowIndex == p.Y && v.ColumnIndex == p.X);
                if (model != null)
                {
                    Squares.Remove(model);
                }
            }
            foreach (Point p in diffs[1])
            {
                if (p.Y < 0) continue;
                Squares.Add(new SquareModel() { Color = tetrimino.Color , RowIndex = (int)p.Y, ColumnIndex = (int)p.X});
            }
            NotifyPropertyChanged(nameof(Squares));
        }

        /// <summary>
        /// テトリミノを移動
        /// </summary>
        /// <param name="action"></param>
        private void Move(Tetrimino.Action action)
        {
            if (tetrimino == null) return;
            Point[][] diffs = tetrimino.GetPointDiffs(action);
            if (!CanMove(diffs[1])) return;
            tetrimino.Move(action);

            SetColor(diffs);

        }

        /// <summary>
        /// ラインがそろっているか確認し、揃っているラインを削除する
        /// </summary>
        private void CheckLine()
        {
            List<SquareModel> list = Squares.ToList();

            for (int row = 19; row >= 0; row--)
            {
                int cnt = list.Count(v => v.RowIndex == row);
                if (cnt == 10)
                {
                    // 該当行のデータを削除
                    list.RemoveAll(v => v.RowIndex == row);
                    // 行をずらす
                    list = list.Select(v => { if (v.RowIndex < row) { v.RowIndex += 1; } return v; }).ToList();
                    // 1行ずらしたので再度同じ行番号をチェックする
                    row += 1;
                    Score += 1;
                    continue;
                }
                else if (cnt == 0)
                {
                    // 空白行 ＝ 上にブロック無
                    break;
                }
            }

            Squares = new ObservableCollection<SquareModel>(list);
        }

    }


}
