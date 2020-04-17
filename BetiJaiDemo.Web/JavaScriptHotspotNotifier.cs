using WebAssembly;

namespace BetiJaiDemo.Web
{
    public class JavaScriptHotspotNotifier : IHotspotNotifier
    {
        public void Notify(string name) => Runtime.InvokeJS($"App.hotspotClicked('{name}');");
    }
}
