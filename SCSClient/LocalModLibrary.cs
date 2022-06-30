using Models.Network;
using System.Collections;

namespace SCSClient
{
    public class LocalModLibrary : IEnumerable<(ModInfo, bool)>
    {

        private List<(ModInfo, bool)> _mods;
        private object _lock; // locks access so the collection can be used on all threads

        public LocalModLibrary()
        {
            _mods = new List<(ModInfo, bool)>();
            _lock = new object();
        }

        // auto locked (as .where is method on ienumberable)
        public (ModInfo, bool) this[string fileHash] => _mods.Where(mod => mod.Item1.FileHash == fileHash).First();

        public void ChangeActivation(string fileHash, bool activation)
        {
            lock(_lock)
            {
                for (int i = 0; i < _mods.Count; i++)
                {
                    if (fileHash == _mods[i].Item1.FileHash)
                    {
                        _mods[i] = (_mods[i].Item1, activation);
                        break;
                    }
                }
            }
        }

        public void AddMod(ModInfo mod, bool activated)
        {
            lock(_lock)
            {
                _mods.Add((mod, activated));
            }
        }

        public IEnumerator<(ModInfo, bool)> GetEnumerator()
        {
            lock(_lock)
            {
                return _mods.ToList().GetEnumerator();
            }
        }

        public bool ContainsMod(string fileHash) => _mods.Where(mod => mod.Item1.FileHash == fileHash).Count() > 0;

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_lock)
            {
                return _mods.ToList().GetEnumerator();
            }
        }
    }
}
