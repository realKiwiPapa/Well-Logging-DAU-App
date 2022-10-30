using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App
{
    public class FileUpload : UploadController
    {
        protected override void UploadCompleted(UploadTask task)
        {
            if (task.hasError) return;
            using (var fileser = new FileServiceHelper())
            {
                task.FileID=fileser.SaveFileUploadInfo(task.Name, task.SHA1, task.MD5, task.Length,task.path);
                if (task.FileID < 0)
                {
                    task.Msg = "文件信息保存出错";
                    task.hasError = true;
                }
            }
        }

        protected override string TaskType
        {
            get { return "FileUpload"; }
        }
    }

    public class FileCacheUpload : FileUpload
    {
        protected override void UploadCompleted(UploadTask task)
        {
        }
    }

    public class PackUpload : UploadController
    {
        protected override void UploadCompleted(UploadTask task)
        {
            // throw new NotImplementedException();
        }

        protected override string TaskType
        {
            get { return "PackUpload"; }
        }
    }
}
