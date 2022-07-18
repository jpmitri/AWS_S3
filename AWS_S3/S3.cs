using System;
using System.IO;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWS_S3
{
    /// <summary>
    /// This Code Is To Help You Implement Amazon S3 With Almost No Hassle
    /// </summary>
    public class S3
    {
        private readonly string Bucket_Name;
        private readonly string awsAccessKeyId;
        private readonly string awsSecretAccessKey;
        private readonly string AWS_Region_String;
        private readonly IAmazonS3 client;
        private readonly Enum_Region Region;
        private readonly RegionEndpoint AWS_Region;
        /// <summary>
        /// The Amazon Region Where Your Server Exists
        /// </summary>
        public enum Enum_Region
        {
            /// <summary>
            /// The US East (Virginia) endpoint.
            /// </summary>
            USEast1 = 0,
            /// <summary>
            /// The Africa (Cape Town) endpoint.
            /// </summary>
            AFSouth1 = 1,
            /// <summary>
            /// The Middle East (Bahrain) endpoint.
            /// </summary>
            MESouth1 = 2,
            /// <summary>
            /// The Canada (Central) endpoint.
            /// </summary>
            CACentral1 = 3,
            /// <summary>
            /// The China (Ningxia) endpoint.
            /// </summary>
            CNNorthWest1 = 4,
            /// <summary>
            /// The China (Beijing) endpoint.
            /// </summary>
            CNNorth1 = 5,
            /// <summary>
            /// The US GovCloud West (Oregon) endpoint.
            /// </summary>
            USGovCloudWest1 = 6,
            /// <summary>
            /// The South America (Sao Paulo) endpoint.
            /// </summary>
            SAEast1 = 7,
            /// <summary>
            /// The Asia Pacific (Sydney) endpoint.
            /// </summary>
            APSoutheast2 = 8,
            /// <summary>
            /// The Asia Pacific (Singapore) endpoint.
            /// </summary>
            APSoutheast1 = 9,
            /// <summary>
            /// The Asia Pacific (Mumbai) endpoint.
            /// </summary>
            APSouth1 = 10,
            /// <summary>
            /// The Asia Pacific (Osaka-Local) endpoint.
            /// </summary>
            APNortheast3 = 11,
            /// <summary>
            /// The US GovCloud East (Virginia) endpoint.
            /// </summary>
            USGovCloudEast1 = 12,
            /// <summary>
            /// The Asia Pacific (Tokyo) endpoint.
            /// </summary>
            APNortheast1 = 13,
            /// <summary>
            /// The Asia Pacific (Seoul) endpoint.
            /// </summary>
            APNortheast2 = 14,
            /// <summary>
            /// The US West (N. California) endpoint.
            /// </summary>
            USWest1 = 15,
            /// <summary>
            /// The US West (Oregon) endpoint.
            /// </summary>
            USWest2 = 16,
            /// <summary>
            /// The EU North (Stockholm) endpoint.
            /// </summary>
            EUNorth1 = 17,
            /// <summary>
            /// The EU West (Ireland) endpoint.
            /// </summary>
            EUWest1 = 18,
            /// <summary>
            /// The US East (Ohio) endpoint.
            /// </summary>
            USEast2 = 19,
            /// <summary>
            /// The EU West (Paris) endpoint.
            /// </summary>
            EUWest3 = 20,
            /// <summary>
            /// The EU Central (Frankfurt) endpoint.
            /// </summary>
            EUCentral1 = 21,
            /// <summary>
            /// The Europe (Milan) endpoint.
            /// </summary>
            EUSouth1 = 22,
            /// <summary>
            /// The Asia Pacific (Hong Kong) endpoint.
            /// </summary>
            APEast1 = 23,
            /// <summary>
            /// The EU West (London) endpoint.
            /// </summary>
            EUWest2 = 24
        }
        /// <summary>
        /// Initialize The Instance Of S3
        /// <example>
        /// <para>For Example:</para>
        /// <code>
        /// S3 aws = new AWS_S3.S3("Bucket1", "ABC123", "123ABC!@#",S3.Enum_Region.MESouth1);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="bucket_Name">The Name Of Your Amazon S3 Bucket Also Known As Main Directory</param>
        /// <param name="awsAccessKeyId">The Access Key ID Given To You By Amazon</param>
        /// <param name="awsSecretAccessKey">The Secret Access Key Given To You By Amazon</param>
        /// <param name="region">The Region Where Your Bucket Resides</param>

        public S3(string bucket_Name, string awsAccessKeyId, string awsSecretAccessKey, Enum_Region region)
        {
            Bucket_Name = bucket_Name ?? throw new ArgumentNullException(nameof(bucket_Name));
            this.awsAccessKeyId = awsAccessKeyId ?? throw new ArgumentNullException(nameof(awsAccessKeyId));
            this.awsSecretAccessKey = awsSecretAccessKey ?? throw new ArgumentNullException(nameof(awsSecretAccessKey));
            Region = region;
            AWS_Region_String = Region.ToString().ToLower();
            AWS_Region_String = AWS_Region_String.Insert(2, "-");
            AWS_Region_String = AWS_Region_String.Insert(AWS_Region_String.Length - 1, "-");
            AWS_Region = Region switch
            {
                Enum_Region.USEast1 => RegionEndpoint.USEast1,
                Enum_Region.AFSouth1 => RegionEndpoint.AFSouth1,
                Enum_Region.MESouth1 => RegionEndpoint.MESouth1,
                Enum_Region.CACentral1 => RegionEndpoint.CACentral1,
                Enum_Region.CNNorthWest1 => RegionEndpoint.CNNorthWest1,
                Enum_Region.CNNorth1 => RegionEndpoint.CNNorth1,
                Enum_Region.USGovCloudWest1 => RegionEndpoint.USGovCloudWest1,
                Enum_Region.SAEast1 => RegionEndpoint.SAEast1,
                Enum_Region.APSoutheast2 => RegionEndpoint.APSoutheast2,
                Enum_Region.APSoutheast1 => RegionEndpoint.APSoutheast1,
                Enum_Region.APSouth1 => RegionEndpoint.APSouth1,
                Enum_Region.APNortheast3 => RegionEndpoint.APNortheast3,
                Enum_Region.USGovCloudEast1 => RegionEndpoint.USGovCloudEast1,
                Enum_Region.APNortheast1 => RegionEndpoint.APNortheast1,
                Enum_Region.APNortheast2 => RegionEndpoint.APNortheast2,
                Enum_Region.USWest1 => RegionEndpoint.USWest1,
                Enum_Region.USWest2 => RegionEndpoint.USWest2,
                Enum_Region.EUNorth1 => RegionEndpoint.EUNorth1,
                Enum_Region.EUWest1 => RegionEndpoint.EUWest1,
                Enum_Region.USEast2 => RegionEndpoint.USEast2,
                Enum_Region.EUWest3 => RegionEndpoint.EUWest3,
                Enum_Region.EUCentral1 => RegionEndpoint.EUCentral1,
                Enum_Region.EUSouth1 => RegionEndpoint.EUSouth1,
                Enum_Region.APEast1 => RegionEndpoint.APEast1,
                Enum_Region.EUWest2 => RegionEndpoint.EUWest2,
                _ => RegionEndpoint.USEast1
            };


            client = new AmazonS3Client(this.awsAccessKeyId, this.awsSecretAccessKey, AWS_Region);
        }
        /// <summary>
        /// Deletes The Item From The Given Bucket
        /// <para>NB: In S3 You Specify the Path Like This Folder1/Folder2/</para>
        /// <example><para>For Example:</para>
        /// <para>My File Is In The Sub Directory Files/App01</para>
        /// <code>
        /// Delete_S3_Item("Test.jpg","Files/App01/")
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="File_Name">The Name Of The File You Want To Delete.</param>
        /// <param name="Folder_Path">The Folder Tree For The Given File.</param>
        /// <returns>True If The Object Was Deleted Successfully</returns>

        public bool Delete_S3_Item(string File_Name, string Folder_Path = "")
        {
            string Location = File_Name;

            if (Folder_Path != "")
            {
                Folder_Path = Folder_Path.Replace("\\", "/");
                Location = Folder_Path + File_Name;
            }

            try
            {
                DeleteObjectRequest delete = new()
                {
                    BucketName = Bucket_Name,
                    Key = Location
                };
                Task<DeleteObjectResponse> deleteObject = client.DeleteObjectAsync(delete);
                deleteObject.Wait();
                return deleteObject.IsCompletedSuccessfully;
            }
            catch (DeleteObjectsException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        /// <summary>
        /// Uploads A File To Your Amazon Bucket With <c>Public Read Permission</c>
        /// <para>NB: In S3 You Specify The Path Like This Folder1/Folder2/</para>
        /// <example>For Example:
        /// <para>
        /// I Want To Upload The File Under "C" Named "img1.jpg" To "My_Bucket/Webimages/TestFolder1"
        /// </para>
        /// <code>
        /// Upload_AWS(@"C:\img1.jpg", "Webimages/TestFolder1/");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="File_Location">The Current Path Of The File To Upload</param>
        /// <param name="Save_Under_Path">The Path In The Bucket Under Which The File Is Saved</param>
        /// <returns>The URL Where You Can Find The File You Just Uploaded</returns>
        public async Task<string> Upload_AWS(string File_Location, string Save_Under_Path = "")
        {
            FileInfo file = new(File_Location);
            string path = Save_Under_Path;
            path += File_Location[index:
                                         File_Location.LastIndexOf("/") >= 0 ?
                                         File_Location.LastIndexOf("/") + 1 :
                                         File_Location.LastIndexOf("\\") + 1
                                 ];
            try
            {
                PutObjectRequest request = new()
                {
                    InputStream = file.OpenRead(),
                    BucketName = Bucket_Name,
                    Key = path,
                    CannedACL = S3CannedACL.PublicRead
                };
                Task<PutObjectResponse> response = client.PutObjectAsync(request);
                await response;
                string link = "https://" + Bucket_Name + ".s3." + AWS_Region_String + ".amazonaws.com/" + path;
                return link;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(e.Message);
                return "Upload Failed";
            }
        }

    }
}
