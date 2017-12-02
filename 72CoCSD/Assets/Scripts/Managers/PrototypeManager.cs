using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using Assets.Scripts.Serialization;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public class PrototypeManager : Singleton<PrototypeManager>
    {
        public List<WordPart> VowelPrototypes;

        public List<WordPart> ConsonantPrototypes;

        public List<CustomerSpawn> CustomerSpawns;

        public IEnumerator LoadPrototypes(Action onLoadCompleted)
        {
            VowelPrototypes = new List<WordPart>();
            var sub = Load<List<WordPart>, WordPart>(VowelPrototypes, "Vowels.xml");
            foreach (var s in sub)
            {
                yield return s;
            }

            ConsonantPrototypes = new List<WordPart>();
            sub = Load<List<WordPart>, WordPart>(ConsonantPrototypes, "Consonants.xml");
            foreach (var s in sub)
            {
                yield return s;
            }

            CustomerSpawns = new List<CustomerSpawn>();
            sub = Load<List<CustomerSpawn>, CustomerSpawn>(CustomerSpawns, "CustomerSpawns.xml");
            foreach (var s in sub)
            {
                yield return s;
            }

            if (onLoadCompleted != null)
            {
                onLoadCompleted();
            }
            yield break;
        }

        private IEnumerable Load<T, TI>(T store, string fileName) where T : class, IList<TI>, new() where TI : class, new()
        {
            var sub = DataSerializer.Instance.LoadFromStreamingAssets<T, TI>(store, "Data", fileName);
            foreach (var s in sub)
            {
                yield return s;
            }
        }
    }
}
