using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FilerWatcherConsoleApp
{
    public class FileSystemWatcher
    {
        private FileInfo _fi;
        private StringBuilder _sb;
        private DirectoryInfo _dirInfo;
        private readonly System.IO.FileSystemWatcher _watch;
        private readonly int _lastLineBuffer;

        public FileSystemWatcher(string watchFolder, int lastLineBuffer)
        {
            _watch = new System.IO.FileSystemWatcher
            {
                // 設定所要監控的資料夾
                Path = watchFolder,
                // 設定所要監控的變更類型
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                // 設定所要監控的檔案類型
                Filter = "*.CSV",
                // 設定是否監控子資料夾
                IncludeSubdirectories = false,
                // 設定是否啟動元件，必須要設定為 true，否則事件是不會被觸發
                EnableRaisingEvents = true
            };
            _lastLineBuffer = lastLineBuffer;
        }

        /// <summary>
        /// 設定監控新增檔案的觸發事件
        /// </summary>
        public FileSystemWatcher WatchCreated()
        {
            _watch.Created += new FileSystemEventHandler(_watch_Created);
            return this;
        }

        /// <summary>
        /// 設定監控修改檔案的觸發事件
        /// </summary>
        public FileSystemWatcher WatchChanged()
        {
            _watch.Changed += new FileSystemEventHandler(_watch_Changed);
            return this;
        }

        /// <summary>
        /// 設定監控重新命名的觸發事件
        /// </summary>
        public FileSystemWatcher WatchRenamed()
        {
            _watch.Renamed += new RenamedEventHandler(_watch_Renamed);
            return this;
        }

        /// <summary>
        /// 設定監控刪除檔案的觸發事件
        /// </summary>
        public FileSystemWatcher WatchDeleted()
        {
            _watch.Deleted += new FileSystemEventHandler(_watch_Deleted);
            return this;
        }

        /// <summary>
        /// 當所監控的資料夾有建立文字檔時觸發
        /// </summary>
        private void _watch_Created(object sender, FileSystemEventArgs e)
        {
            _sb = new StringBuilder();

            _dirInfo = new DirectoryInfo(e.FullPath);

            _sb.AppendLine($"新建檔案於：{_dirInfo.FullName.Replace(_dirInfo.Name, "")}");
            _sb.AppendLine($"新建檔案名稱：{_dirInfo.Name}");
            _sb.AppendLine($"建立時間：{_dirInfo.CreationTime}");
            _sb.AppendLine($"目錄下共有：{_dirInfo.Parent?.GetFiles().Length} 檔案");
            _sb.AppendLine($"目錄下共有：{_dirInfo.Parent?.GetDirectories().Length} 資料夾");

            Console.WriteLine(_sb.ToString());
        }

        /// <summary>
        /// 當所監控的資料夾有文字檔檔案內容有異動時觸發
        /// </summary>
        private void _watch_Changed(object sender, FileSystemEventArgs e)
        {
            _sb = new StringBuilder();

            _dirInfo = new DirectoryInfo(e.FullPath);

            _sb.AppendLine($"被異動的檔名為：{e.Name}");
            _sb.AppendLine($"檔案所在位址為：{e.FullPath.Replace(e.Name, "")}");
            _sb.AppendLine($"異動內容時間為：{_dirInfo.LastWriteTime}");
            _sb.AppendLine($"異動內容：");

            Console.WriteLine(_sb.ToString());

            foreach (var line in File.ReadLines(e.FullPath).TakeLast(_lastLineBuffer))
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// 當所監控的資料夾有文字檔檔案重新命名時觸發
        /// </summary>
        private void _watch_Renamed(object sender, RenamedEventArgs e)
        {
            _sb = new StringBuilder();

            _fi = new FileInfo(e.FullPath);

            _sb.AppendLine($"檔名更新前：{e.OldName}");
            _sb.AppendLine($"檔名更新後：{e.Name}");
            _sb.AppendLine($"檔名更新前路徑：{e.OldFullPath}");
            _sb.AppendLine($"檔名更新後路徑：{e.FullPath}");
            _sb.AppendLine($"建立時間：{_fi.LastAccessTime}");

            Console.WriteLine(_sb.ToString());
        }

        /// <summary>
        /// 當所監控的資料夾有文字檔檔案有被刪除時觸發
        /// </summary>
        private void _watch_Deleted(object sender, FileSystemEventArgs e)
        {
            _sb = new StringBuilder();

            _sb.AppendLine($"被刪除的檔名為：{e.Name}");
            _sb.AppendLine($"檔案所在位址為：{e.FullPath.Replace(e.Name, "")}");
            _sb.AppendLine($"刪除時間：{DateTime.Now}");

            Console.WriteLine(_sb.ToString());
        }
    }
}
