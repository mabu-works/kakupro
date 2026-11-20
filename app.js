'use strict';

/* ===== プール ===== */

const SURNAMES = [
'佐藤', '鈴木', '高橋', '田中', '渡辺', '伊藤', '山本', '中村', '小林', '加藤',
'吉田', '山田', '佐々木', '山口', '松本', '井上', '木村', '林', '清水', '山崎',
'森', '池田', '阿部', '橋本', '山下', '石川', '中島', '石井', '前田', '藤田', '小川'
];

const GIVEN_NAMES = [
'大翔', '陽翔', '蓮', '湊', '蒼', '樹', '悠真', '陽太', '健太', '翔太',
'陸', '海斗', '健一', '直樹', '翼', '悠',
'葵', '結菜', '陽葵', '莉子', '芽依', '澪', '凛', '紬', '美咲', '七海',
'さくら', 'ひなた', '楓', '裕子'
];

const JOBS = [
'会社員', '教師', '医者', '看護師', 'エンジニア', 'デザイナー', '農家', '漁師',
'料理人', 'パン屋', '声優', '作家', '漫画家', '音楽家', '画家', '写真家',
'美容師', '大工', '電気工事士', '警察官', '消防士', 'パイロット', '宇宙飛行士',
'研究者', '翻訳者', '司書', '保育士', '弁護士', '会計士', 'スポーツ選手',
'動画配信者', '占い師', '探偵', '花屋', '寿司職人', '駅員'
];

const SKILLS = [
'早起き', '暗算', '料理', '歌', 'ダンス', '絵を描くこと', '速読', '抜群の記憶力',
'方向感覚', '語学', '楽器演奏', 'スポーツ全般', '裁縫', 'DIY', 'プログラミング',
'将棋', 'けん玉', 'ものまね', '書道', '水泳'
];

const LIKES = [
'猫', '犬', '読書', '映画', '音楽', 'ゲーム', '旅行', '甘いもの',
'コーヒー', '紅茶', '海', '山', '星空', '温泉', 'ラーメン', '寿司',
'散歩', '昼寝', '写真', '植物'
];

const DISLIKES = [
'虫', '高いところ', '暗いところ', '辛いもの', '早起き', '満員電車', 'ホラー映画',
'注射', '大勢の前で話すこと', '運動', '数学', '片付け', '暑さ', '寒さ',
'待ち時間', '渋滞', '人混み', 'ピーマン'
];

const PHRASES = [
'なるほどね', 'まあいっか', 'とりあえず', '実はね', '〜なんだよね', '知らんけど',
'まじで？', 'おっけー', 'なるほどなるほど', 'だよね〜', 'とりあえずお茶',
'まあまあ', 'そういうことか', 'ほんとそれ', '一旦落ち着こう', 'なんとかなる'
];

const COMMENTS = [
'よろしくお願いします！', '毎日が冒険です', 'のんびり屋ですが頑張ります',
'好奇心旺盛です', '食べることが生きがいです', '笑顔が取り柄です',
'マイペースに生きています', '新しいことに挑戦中です', '人と話すのが大好きです',
'静かに燃えるタイプです', '趣味に生きています', '明日は明日の風が吹く',
'一期一会を大切にしています', '継続は力なり、が座右の銘', 'いつも眠いです',
'推し活が生きがいです'
];

/* ===== 項目定義 ===== */

function pick(arr) { return arr[Math.floor(Math.random() * arr.length)];}

const FIELDS = [
{ key: 'name', label: '名前', generate: () => pick(SURNAMES) + ' ' + pick(GIVEN_NAMES)},
{ key: 'age', label: '年齢', generate: () => String(6 + Math.floor(Math.random() * 84)) + '歳'},
{ key: 'job', label: '職業', generate: () => pick(JOBS)},
{ key: 'skill', label: '特技', generate: () => pick(SKILLS)},
{ key: 'like', label: '好きなもの', generate: () => pick(LIKES)},
{ key: 'dislike', label: '苦手なもの', generate: () => pick(DISLIKES)},
{ key: 'phrase', label: '口癖', generate: () => '「' + pick(PHRASES) + '」'},
{ key: 'comment', label: '一言コメント', generate: () => pick(COMMENTS)}
];

const state = {}; // key -> { value, locked}
for (const f of FIELDS) state[f.key] = { value: f.generate(), locked: false};

/* ===== DOM 描画 ===== */

const $ = (id) => document.getElementById(id);
const listEl = $('profile-list');
const statusEl = $('status');
const shareEl = $('share-text');

function setStatus(msg) { statusEl.textContent = msg;}

function buildRows() {
listEl.innerHTML = '';
for (const f of FIELDS) {
const row = document.createElement('div');
row.className = 'profile-row';

const label = document.createElement('span');
label.className = 'label';
label.textContent = f.label;

const value = document.createElement('span');
value.className = 'value';
value.id = 'value-' + f.key;
value.textContent = state[f.key].value;

const lockLabel = document.createElement('label');
lockLabel.className = 'lock';
const lockBox = document.createElement('input');
lockBox.type = 'checkbox';
lockBox.id = 'lock-' + f.key;
lockBox.addEventListener('change', () => {
state[f.key].locked = lockBox.checked;
setStatus(f.label + 'を' + (lockBox.checked? '固定しました。': '固定を解除しました。'));
});
lockLabel.append(lockBox, document.createTextNode('固定'));

const reroll = document.createElement('button');
reroll.type = 'button';
reroll.className = 'reroll';
reroll.textContent = '再抽選';
reroll.addEventListener('click', () => {
state[f.key].value = f.generate();
refresh();
setStatus(f.label + 'を引き直しました。');
});

row.append(label, value, lockLabel, reroll);
listEl.appendChild(row);
}
}

function profileText() {
const lines = [''];
for (const f of FIELDS) lines.push(f.label + '：' + state[f.key].value);
lines.push('#kakupro');
return lines.join('\n');
}

function refresh() {
for (const f of FIELDS) {
$('value-' + f.key).textContent = state[f.key].value;
}
shareEl.value = profileText();
drawCard();
}

/* ===== カード描画（Canvas） ===== */

function drawCard() {
const canvas = $('card-canvas');
const ctx = canvas.getContext('2d');
const W = 1200, H = 630;

ctx.fillStyle = '#123a3f';
ctx.fillRect(0, 0, W, H);
ctx.fillStyle = '#0e7c86';
ctx.fillRect(0, 0, W, 14);
ctx.strokeStyle = '#2dd4bf';
ctx.lineWidth = 3;
ctx.strokeRect(24, 24, W - 48, H - 48);

ctx.fillStyle = '#7dd3d8';
ctx.font = '700 28px sans-serif';
ctx.fillText('架空プロフィール', 64, 88);

ctx.fillStyle = '#ffffff';
ctx.font = '700 64px sans-serif';
ctx.fillText(state.name.value, 64, 168);

ctx.font = '30px sans-serif';
const rows = [
['年齢', state.age.value], ['職業', state.job.value],
['特技', state.skill.value], ['好きなもの', state.like.value],
['苦手なもの', state.dislike.value], ['口癖', state.phrase.value]
];
let y = 236;
for (const [label, value] of rows) {
ctx.fillStyle = '#7dd3d8';
ctx.font = '700 28px sans-serif';
ctx.fillText(label, 64, y);
ctx.fillStyle = '#ffffff';
ctx.font = '30px sans-serif';
ctx.fillText(value, 250, y);
y += 48;
}

/* 一言コメントとクレジットが重ならないよう、行間を詰めて下部に余白を確保する */
ctx.fillStyle = '#fde68a';
ctx.font = '700 30px sans-serif';
ctx.fillText('「' + state.comment.value.replace(/^「|」$/g, '') + '」', 64, y + 22);

ctx.fillStyle = '#8fb7ba';
ctx.font = '24px sans-serif';
ctx.fillText('kakupro — 架空プロフィール生成器', 64, H - 40);
}

/* ===== ボタン ===== */

$('btn-regen').addEventListener('click', () => {
for (const f of FIELDS) {
if (!state[f.key].locked) state[f.key].value = f.generate();
}
refresh();
setStatus('固定した項目以外を再生成しました。');
});

$('btn-all-new').addEventListener('click', () => {
for (const f of FIELDS) state[f.key].value = f.generate();
refresh();
setStatus('全部の項目を作り直しました（固定チェックはそのままです）。');
});

$('btn-card').addEventListener('click', () => {
drawCard();
const a = document.createElement('a');
a.download = 'kakupro-card.png';
a.href = $('card-canvas').toDataURL('image/png');
a.click();
setStatus('プロフィールカードをダウンロードしました。');
});

$('btn-copy').addEventListener('click', async () => {
try {
await navigator.clipboard.writeText(shareEl.value);
setStatus('テキストをコピーしました。');
} catch (e) {
shareEl.select();
document.execCommand('copy');
setStatus('テキストをコピーしました。');
}
});

/* ===== テーマ切り替え（見た目のみ・生成ロジックには触れない） ===== */

(function initTheme() {
  var root = document.documentElement;
  if (!root || !root.getAttribute || !root.setAttribute) return;
  function apply(theme) {
    root.setAttribute("data-theme", theme);
    var btn = document.getElementById("themeToggle");
    if (btn) btn.textContent = theme === "light" ? "☀️" : "🌙";
  }
  var current = root.getAttribute("data-theme");
  if (current !== "light" && current !== "dark") {
    try { current = localStorage.getItem("kakupro-theme"); } catch (e) { current = null; }
  }
  apply(current === "light" ? "light" : "dark");
  var btn = document.getElementById("themeToggle");
  if (btn && btn.addEventListener) {
    btn.addEventListener("click", function () {
      var next = root.getAttribute("data-theme") === "light" ? "dark" : "light";
      apply(next);
      try { localStorage.setItem("kakupro-theme", next); } catch (e) { /* 保存できなくても切り替え自体は有効 */ }
    });
  }
})();

/* ===== 初期化 ===== */

buildRows();
refresh();
