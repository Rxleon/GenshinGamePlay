/* this is generated by nino */
namespace TaoTie
{
    public partial class ConfigGearAddExtraGroupAction
    {
        public static ConfigGearAddExtraGroupAction.SerializationHelper NinoSerializationHelper = new ConfigGearAddExtraGroupAction.SerializationHelper();
        public class SerializationHelper: Nino.Serialization.NinoWrapperBase<ConfigGearAddExtraGroupAction>
        {
            #region NINO_CODEGEN
            public override void Serialize(ConfigGearAddExtraGroupAction value, Nino.Serialization.Writer writer)
            {
                if(value == null)
                {
                    writer.Write(false);
                    return;
                }
                writer.Write(true);
                writer.Write(value.disable);
                writer.CompressAndWrite(ref value.localId);
                writer.Write(value.isOtherGear);
                writer.CompressAndWrite(ref value.otherGearId);
                writer.CompressAndWrite(ref value.groupId);
            }

            public override ConfigGearAddExtraGroupAction Deserialize(Nino.Serialization.Reader reader)
            {
                if(!reader.ReadBool())
                    return null;
                ConfigGearAddExtraGroupAction value = new ConfigGearAddExtraGroupAction();
                reader.Read<System.Boolean>(ref value.disable, 1);
                reader.DecompressAndReadNumber<System.Int32>(ref value.localId);
                reader.Read<System.Boolean>(ref value.isOtherGear, 1);
                reader.DecompressAndReadNumber<System.UInt64>(ref value.otherGearId);
                reader.DecompressAndReadNumber<System.Int32>(ref value.groupId);
                return value;
            }
            #endregion
        }
    }
}