/* this is generated by nino */
namespace TaoTie
{
    public partial class ConfigVariableChangeEventTrigger
    {
        public static ConfigVariableChangeEventTrigger.SerializationHelper NinoSerializationHelper = new ConfigVariableChangeEventTrigger.SerializationHelper();
        public class SerializationHelper: Nino.Serialization.NinoWrapperBase<ConfigVariableChangeEventTrigger>
        {
            #region NINO_CODEGEN
            public override void Serialize(ConfigVariableChangeEventTrigger value, Nino.Serialization.Writer writer)
            {
                if(value == null)
                {
                    writer.Write(false);
                    return;
                }
                writer.Write(true);
                writer.CompressAndWrite(ref value.localId);
                writer.Write(value.actions);
                writer.Write(value.key);
            }

            public override ConfigVariableChangeEventTrigger Deserialize(Nino.Serialization.Reader reader)
            {
                if(!reader.ReadBool())
                    return null;
                ConfigVariableChangeEventTrigger value = new ConfigVariableChangeEventTrigger();
                reader.DecompressAndReadNumber<System.Int32>(ref value.localId);
                value.actions = reader.ReadArray<TaoTie.ConfigGearAction>();
                value.key = reader.ReadString();
                return value;
            }
            #endregion
        }
    }
}