using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Kakupro
{
    internal static class Program
    {
        // 🚀 アプリの入口（フォームを起動する）
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }

    // ==========================================
    // 🖥️ メイン画面
    // ==========================================

    internal sealed class MainForm : Form
    {
        // ==========================================
        // 🎲 生成データのプール（Web版と同じ内容）
        // ==========================================
        private static readonly string[] Surnames =
        {
            "佐藤", "鈴木", "高橋", "田中", "渡辺", "伊藤",
            "山本", "中村", "小林", "加藤", "吉田", "山田",
            "佐々木", "山口", "松本", "井上", "木村", "林",
            "清水", "山崎", "森", "池田", "阿部", "橋本",
            "山下", "石川", "中島", "石井", "前田", "藤田",
            "小川",
        };

        private static readonly string[] GivenNames =
        {
            "大翔", "陽翔", "蓮", "湊", "蒼", "樹",
            "悠真", "陽太", "健太", "翔太", "陸", "海斗",
            "健一", "直樹", "翼", "悠", "葵", "結菜",
            "陽葵", "莉子", "芽依", "澪", "凛", "紬",
            "美咲", "七海", "さくら", "ひなた", "楓", "裕子",
        };

        private static readonly string[] Jobs =
        {
            "会社員", "教師", "医者", "看護師", "エンジニア", "デザイナー",
            "農家", "漁師", "料理人", "パン屋", "声優", "作家",
            "漫画家", "音楽家", "画家", "写真家", "美容師", "大工",
            "電気工事士", "警察官", "消防士", "パイロット", "宇宙飛行士", "研究者",
            "翻訳者", "司書", "保育士", "弁護士", "会計士", "スポーツ選手",
            "動画配信者", "占い師", "探偵", "花屋", "寿司職人", "駅員",
        };

        private static readonly string[] Skills =
        {
            "早起き", "暗算", "料理", "歌", "ダンス", "絵を描くこと",
            "速読", "抜群の記憶力", "方向感覚", "語学", "楽器演奏", "スポーツ全般",
            "裁縫", "DIY", "プログラミング", "将棋", "けん玉", "ものまね",
            "書道", "水泳",
        };

        private static readonly string[] Likes =
        {
            "猫", "犬", "読書", "映画", "音楽", "ゲーム",
            "旅行", "甘いもの", "コーヒー", "紅茶", "海", "山",
            "星空", "温泉", "ラーメン", "寿司", "散歩", "昼寝",
            "写真", "植物",
        };

        private static readonly string[] Dislikes =
        {
            "虫", "高いところ", "暗いところ", "辛いもの", "早起き", "満員電車",
            "ホラー映画", "注射", "大勢の前で話すこと", "運動", "数学", "片付け",
            "暑さ", "寒さ", "待ち時間", "渋滞", "人混み", "ピーマン",
        };

        private static readonly string[] Phrases =
        {
            "なるほどね", "まあいっか", "とりあえず", "実はね", "〜なんだよね", "知らんけど",
            "まじで？", "おっけー", "なるほどなるほど", "だよね〜", "とりあえずお茶", "まあまあ",
            "そういうことか", "ほんとそれ", "一旦落ち着こう", "なんとかなる",
        };

        private static readonly string[] Comments =
        {
            "よろしくお願いします！", "毎日が冒険です", "のんびり屋ですが頑張ります", "好奇心旺盛です", "食べることが生きがいです", "笑顔が取り柄です",
            "マイペースに生きています", "新しいことに挑戦中です", "人と話すのが大好きです", "静かに燃えるタイプです", "趣味に生きています", "明日は明日の風が吹く",
            "一期一会を大切にしています", "継続は力なり、が座右の銘", "いつも眠いです", "推し活が生きがいです",
        };

        // 📋 項目定義
        private sealed class FieldDef
        {
            public string Key = "";
            public string Label = "";
            public Func<string> Generate = () => "";
        }

        // 🎲 プールから1件ランダムに選ぶ
        private static string Pick(string[] arr) => arr[RandomNumberGenerator.GetInt32(arr.Length)];

        // 🗂️ 生成する項目一覧
        private static readonly FieldDef[] Fields =
        {
            new FieldDef { Key = "name", Label = "名前", Generate = () => Pick(Surnames) + " " + Pick(GivenNames) },
            new FieldDef { Key = "age", Label = "年齢", Generate = () => (6 + RandomNumberGenerator.GetInt32(84)) + "歳" }, // 6〜89歳
            new FieldDef { Key = "job", Label = "職業", Generate = () => Pick(Jobs) },
            new FieldDef { Key = "skill", Label = "特技", Generate = () => Pick(Skills) },
            new FieldDef { Key = "like", Label = "好きなもの", Generate = () => Pick(Likes) },
            new FieldDef { Key = "dislike", Label = "苦手なもの", Generate = () => Pick(Dislikes) },
            new FieldDef { Key = "phrase", Label = "口癖", Generate = () => "「" + Pick(Phrases) + "」" },
            new FieldDef { Key = "comment", Label = "一言コメント", Generate = () => Pick(Comments) },
        };

        // 🎨 画面の配色
        private static readonly Color Bg = Color.FromArgb(10, 15, 28);
        private static readonly Color Surface = Color.FromArgb(17, 26, 46);
        private static readonly Color TextColor = Color.FromArgb(236, 241, 248);
        private static readonly Color Muted = Color.FromArgb(157, 177, 199);
        private static readonly Color Accent = Color.FromArgb(106, 141, 255);
        private static readonly Color Accent2 = Color.FromArgb(45, 212, 191);

        // 🗝️ 項目ごとの値と表示部品
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();
        private readonly Dictionary<string, Label> _valueLabels = new Dictionary<string, Label>();
        private readonly Dictionary<string, CheckBox> _lockBoxes = new Dictionary<string, CheckBox>();
        private readonly Label statusLabel;
        private readonly TextBox shareTextBox;

        // 🧱 画面を組み立てる
        public MainForm()
        {
            Text = "kakupro for Windows｜架空プロフィール生成器";
            ClientSize = new Size(640, 736);
            MinimumSize = new Size(656, 775);
            BackColor = Bg;
            ForeColor = TextColor;
            Font = new Font("Yu Gothic UI", 10F);

            var title = new Label
            {
                Text = "kakupro｜架空プロフィール生成器",
                Font = new Font("Yu Gothic UI", 16F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(20, 14),
                AutoSize = true
            };
            Controls.Add(title);

            // 🌱 開いた時点で1人分を生成する
            foreach (var f in Fields) _values[f.Key] = f.Generate();

            int y = 60;
            foreach (var f in Fields)
            {
                var field = f;
                var label = new Label
                {
                    Text = field.Label,
                    ForeColor = Muted,
                    Location = new Point(20, y + 3),
                    Size = new Size(92, 24)
                };
                Controls.Add(label);

                var value = new Label
                {
                    Text = _values[field.Key],
                    ForeColor = TextColor,
                    Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold),
                    Location = new Point(116, y + 2),
                    Size = new Size(330, 24)
                };
                _valueLabels[field.Key] = value;
                Controls.Add(value);

                var lockBox = new CheckBox
                {
                    Text = "固定",
                    ForeColor = TextColor,
                    Location = new Point(452, y + 2),
                    Size = new Size(60, 24)
                };
                lockBox.CheckedChanged += (s, e) =>
                {
                    SetStatus(field.Label + "を" + (lockBox.Checked ? "固定しました。" : "固定を解除しました。"));
                };
                _lockBoxes[field.Key] = lockBox;
                Controls.Add(lockBox);

                var reroll = new Button
                {
                    Text = "再抽選",
                    BackColor = Surface,
                    ForeColor = TextColor,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(520, y),
                    Size = new Size(90, 28)
                };
                reroll.FlatAppearance.BorderSize = 0;
                reroll.Click += (s, e) =>
                {
                    _values[field.Key] = field.Generate();
                    RefreshProfile();
                    SetStatus(field.Label + "を引き直しました。");
                };
                Controls.Add(reroll);

                y += 36;
            }

            // 🔄 固定以外を再生成する
            var regenBtn = new Button
            {
                Text = "再生成（固定以外）",
                Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold),
                BackColor = Accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 360),
                Size = new Size(240, 36)
            };
            regenBtn.FlatAppearance.BorderSize = 0;
            regenBtn.Click += (s, e) =>
            {
                foreach (var f in Fields)
                {
                    if (!_lockBoxes[f.Key].Checked) _values[f.Key] = f.Generate();
                }
                RefreshProfile();
                SetStatus("固定した項目以外を再生成しました。");
            };
            Controls.Add(regenBtn);

            // 🎲 全部の項目を作り直す
            var allNewBtn = SubButton("全部ランダムに作り直す", 272, 360, 240);
            allNewBtn.Click += (s, e) =>
            {
                foreach (var f in Fields) _values[f.Key] = f.Generate();
                RefreshProfile();
                SetStatus("全部の項目を作り直しました（固定チェックはそのままです）。");
            };
            Controls.Add(allNewBtn);

            var note = new Label
            {
                Text = "各項目の「固定」にチェックを入れると、再生成してもその項目は変わりません。「再抽選」はその項目だけを引き直します。",
                ForeColor = Muted,
                Font = new Font("Yu Gothic UI", 9F),
                Location = new Point(20, 406),
                Size = new Size(600, 30)
            };
            Controls.Add(note);

            statusLabel = new Label
            {
                Text = "",
                ForeColor = Accent2,
                Location = new Point(20, 440),
                AutoSize = true
            };
            Controls.Add(statusLabel);

            var shareTitle = new Label
            {
                Text = "共有する",
                Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(20, 470),
                AutoSize = true
            };
            Controls.Add(shareTitle);

            // 🖼️ カード画像を保存する
            var cardBtn = new Button
            {
                Text = "カード画像を保存",
                Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold),
                BackColor = Accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 502),
                Size = new Size(240, 36)
            };
            cardBtn.FlatAppearance.BorderSize = 0;
            cardBtn.Click += (s, e) => SaveCardImage();
            Controls.Add(cardBtn);

            // 📋 共有テキストをコピーする
            var copyBtn = SubButton("テキストをコピー（X投稿向け）", 272, 502, 240);
            copyBtn.Click += (s, e) =>
            {
                try { Clipboard.SetText(shareTextBox.Text); } catch { /* コピーできない環境では無視 */ }
                SetStatus("テキストをコピーしました。");
            };
            Controls.Add(copyBtn);

            var shareLabel = new Label
            {
                Text = "コピーされるテキスト",
                ForeColor = Muted,
                Location = new Point(20, 548),
                AutoSize = true
            };
            Controls.Add(shareLabel);

            shareTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(20, 574),
                Size = new Size(600, 148),
                BackColor = Surface,
                ForeColor = TextColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(shareTextBox);

            RefreshProfile();
        }

        // 🔘 補助ボタン（共通の見た目）を作る
        private Button SubButton(string text, int x, int y, int w)
        {
            var b = new Button
            {
                Text = text,
                BackColor = Surface,
                ForeColor = TextColor,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(x, y),
                Size = new Size(w, 36)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        // 📣 ステータス文を表示する
        private void SetStatus(string msg) => statusLabel.Text = msg;

        // 📝 共有用のプロフィール文を作る
        private string ProfileText()
        {
            var lines = new List<string> { "" };
            foreach (var f in Fields) lines.Add(f.Label + "：" + _values[f.Key]);
            lines.Add("#kakupro");
            return string.Join(Environment.NewLine, lines);
        }

        // 🔄 表示と共有テキストを更新する
        private void RefreshProfile()
        {
            foreach (var f in Fields) _valueLabels[f.Key].Text = _values[f.Key];
            shareTextBox.Text = ProfileText();
        }

        // 🖼️ Web版 drawCard と同じレイアウト・配色でカード画像（1200×630）を描く
        private void SaveCardImage()
        {
            using var bmp = new Bitmap(1200, 630);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(ColorTranslator.FromHtml("#123a3f"));
                using (var topBrush = new SolidBrush(ColorTranslator.FromHtml("#0e7c86")))
                    g.FillRectangle(topBrush, 0, 0, 1200, 14);
                using (var pen = new Pen(ColorTranslator.FromHtml("#2dd4bf"), 3))
                    g.DrawRectangle(pen, 24, 24, 1200 - 48, 630 - 48);

                var headBrush = new SolidBrush(ColorTranslator.FromHtml("#7dd3d8"));
                var whiteBrush = new SolidBrush(Color.White);
                var titleFont = new Font("Yu Gothic UI", 28F, FontStyle.Bold);
                var nameFont = new Font("Yu Gothic UI", 64F, FontStyle.Bold);
                var rowHeadFont = new Font("Yu Gothic UI", 28F, FontStyle.Bold);
                var rowFont = new Font("Yu Gothic UI", 30F);
                // 📐 Canvas の fillText はベースライン指定・GDI+ は上端指定のためフォントサイズ分ずらす
                g.DrawString("架空プロフィール", titleFont, headBrush, 64, 88 - 34);
                g.DrawString(_values["name"], nameFont, whiteBrush, 64, 168 - 78);

                var rowKeys = new[] { "age", "job", "skill", "like", "dislike", "phrase" };
                int y = 236;
                foreach (var key in rowKeys)
                {
                    var def = Array.Find(Fields, f => f.Key == key)!;
                    g.DrawString(def.Label, rowHeadFont, headBrush, 64, y - 34);
                    g.DrawString(_values[key], rowFont, whiteBrush, 250, y - 36);
                    y += 48;
                }

                var commentFont = new Font("Yu Gothic UI", 30F, FontStyle.Bold);
                var commentBrush = new SolidBrush(ColorTranslator.FromHtml("#fde68a"));
                g.DrawString("「" + _values["comment"] + "」", commentFont, commentBrush, 64, y + 22 - 36);

                var creditFont = new Font("Yu Gothic UI", 24F);
                var creditBrush = new SolidBrush(ColorTranslator.FromHtml("#8fb7ba"));
                g.DrawString("kakupro — 架空プロフィール生成器", creditFont, creditBrush, 64, 630 - 40 - 28);
            }

            using var dlg = new SaveFileDialog
            {
                Filter = "PNG画像|*.png",
                FileName = "kakupro-card.png",
                Title = "プロフィールカードを保存"
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                bmp.Save(dlg.FileName, ImageFormat.Png);
                SetStatus("プロフィールカードを保存しました。");
            }
        }
    }
}
