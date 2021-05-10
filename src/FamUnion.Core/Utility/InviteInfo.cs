using System;
using System.IO;
using System.Web;

namespace FamUnion.Core.Utility
{
    public class InviteInfo
    {
        public Guid InviteId { get; set; }
        public Guid ReunionId { get; set; } = Guid.Empty;
        public DateTime ExpiresAt { get; set; } = DateTime.MinValue;
        public string InviteEmail { get; set; } = string.Empty;

        public bool IsValid()
        {
            return ReunionId != Guid.Empty
                && ExpiresAt != DateTime.MinValue
                && !string.IsNullOrWhiteSpace(InviteEmail);
        }

        public string Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter bwriter = new BinaryWriter(stream);
            Random r = new Random();
            int bs = r.Next(1, 255);  // a randomized byte to prepend after Id
            bwriter.Write(InviteId.ToString());
            bwriter.Write((byte)bs);
            bwriter.Write(ReunionId.ToString());
            bs = r.Next(1, 255); // a randomized byte to append after ReunionId
            bwriter.Write((byte)bs);
            bwriter.Write(ExpiresAt.ToShortDateString());
            bs = r.Next(1, 255); // a randomized byte to append after ExpiresAt
            bwriter.Write(InviteEmail);
            bs = r.Next(1, 255); // a randomized byte to append after InviteEmail
            bwriter.Write(bs);  
            bwriter.Close();
            return HttpUtility.UrlEncode(Convert.ToBase64String(stream.ToArray()));
        }

        public static string Encode(InviteInfo invite)
        {
            return invite.Encode();
        }

        public static InviteInfo Decode(string EncodedString)
        {
            byte[] encodedBytes = Convert.FromBase64String(EncodedString);
            MemoryStream stream = new MemoryStream(encodedBytes);
            BinaryReader breader = new BinaryReader(stream);
            InviteInfo inviteInfo = new InviteInfo();
            inviteInfo.InviteId = Guid.Parse(breader.ReadString());
            breader.ReadByte();
            inviteInfo.ReunionId = Guid.Parse(breader.ReadString());
            breader.ReadByte();
            inviteInfo.ExpiresAt = DateTime.Parse(breader.ReadString());
            breader.ReadByte();
            inviteInfo.InviteEmail = breader.ReadString();

            return inviteInfo;
        }
    }
}
