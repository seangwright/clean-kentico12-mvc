using System.Linq;
using System.Threading.Tasks;

namespace CMS.DataEngine
{
    public static class ObjectQueryExtensions
    {
        public static async Task<TObject> FirstOrDefaultAsync<TObject>(this ObjectQuery<TObject> query)
            where TObject : BaseInfo
            => (await query.GetEnumerableTypedResultAsync()).FirstOrDefault();

        public static async Task<TObject> SingleAsync<TObject>(this ObjectQuery<TObject> query)
            where TObject : BaseInfo
            => (await query.GetEnumerableTypedResultAsync()).Single();
    }
}
