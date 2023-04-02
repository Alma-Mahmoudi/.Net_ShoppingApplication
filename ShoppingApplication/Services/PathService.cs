using ShoppingApplication.Options;

namespace ShoppingApplication.Services
{
    public class PathService
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;
        public PathService(IConfiguration configuration,IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env; 
        }

        public string GetUploadsPath(string filename=null,bool withWebRoothPath=true)
        {
            var pathOptions = new PathOptions();

            configuration.GetSection(PathOptions.Path).Bind(pathOptions);

            var uploadsPath = pathOptions.ArticlesImages;

            if(null != filename)
            {
                uploadsPath = Path.Combine(uploadsPath, filename);
            }
            return withWebRoothPath ? Path.Combine(env.WebRootPath, uploadsPath) : uploadsPath;
        }
    }
}
