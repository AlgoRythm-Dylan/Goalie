using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Goalie.Lib
{
    public class DataDir
    {
        public DataDir Parent { get; set; }
        private string _path;
        public string Path
        {
            get
            {
                if (Parent != null)
                    return System.IO.Path.Combine(Parent.Path, _path);
                else
                    return _path;
            }
            set
            {
                _path = value;
            }
        }
        public DataDir(string path, DataDir parent)
        {
            Path = path;
            Parent = parent;
        }
        public DataDir(string path)
        {
            Path = path;
            Parent = null;
        }
        public bool Ensure()
        {
            if (Parent != null && !Parent.Ensure())
                return false;
            if (Directory.Exists(Path))
                return true;
            try
            {
                Directory.CreateDirectory(Path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
        public async Task<List<T>> ReadAllItemsAsync<T>()
        {
            var list = new List<T>();
            if (Ensure())
            {
                foreach (string file in Directory.GetFiles(Path))
                {
                    try
                    {
                        list.Add(JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(file)));
                    }
                    catch { } // We mostly just want to continue past any interruptions
                }
            }
            return list;
        }

    }
}
