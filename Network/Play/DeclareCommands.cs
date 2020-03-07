using System;

namespace SM3.Network.Play
{
    public readonly struct DeclareCommands : IWriteablePacket
    {
        public int Id => 0x12;

        public readonly ICommandProvider.CommandInfo[] CommandInfos;

        public DeclareCommands(ICommandProvider.CommandInfo[] commandInfos)
        {
            CommandInfos = commandInfos;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(CommandInfos.Length);

            foreach (var cmd in CommandInfos)
            {
                var flags = (byte)cmd.NodeType; // not 100% safe
                if (cmd.IsExecutable)
                    flags |= 0x04;

                if (cmd.RedirectId != null)
                    flags |= 0x08;

                if (cmd.Parser?.SuggestionType != null)
                    flags |= 0x10;
                
                writer.WriteUInt8(flags);
                writer.WriteVarInt(cmd.Children.Length);
                for (int i = 0; i < cmd.Children.Length; i++)
                    writer.WriteVarInt(cmd.Children[i]);
                
                if (cmd.RedirectId != null)
                    writer.WriteVarInt((int)cmd.RedirectId);
                
                if (cmd.Name != null)
                    writer.WriteString(cmd.Name);

                if (cmd.Parser != null)
                {
                    throw new NotImplementedException();
                    /*writer.WriteString(cmd.Parser.Id);
                    cmd.Parser.SerializeProperties(writer);
                    if (cmd.Parser.SuggestionType != null)
                        writer.WriteString(cmd.Parser.SuggestionType);*/
                }
            }
            
            writer.WriteVarInt(CommandInfos.Length - 1);
        }
    }
}