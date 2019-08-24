namespace MeadowCLI
{
    public static class OptionsExtensions
    {
        public static bool IsFileOperation(this Options o)
        {
            if (o.WriteFile) return true;
            if (o.DeleteFile) return true;
            if (o.EraseFlash) return true;
            if (o.VerifyErasedFlash) return true;
            if (o.PartitionFileSystem) return true;
            if (o.MountFileSystem) return true;
            if (o.InitFileSystem) return true;
            if (o.CreateFileSystem) return true;
            if (o.FormatFileSystem) return true;

            return false;
        }
    }
}