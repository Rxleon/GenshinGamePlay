/* this is generated by nino */
namespace TaoTie
{
    public partial class ConfigAttachToAfterBeAttack
    {
        public static ConfigAttachToAfterBeAttack.SerializationHelper NinoSerializationHelper = new ConfigAttachToAfterBeAttack.SerializationHelper();
        public class SerializationHelper: Nino.Serialization.NinoWrapperBase<ConfigAttachToAfterBeAttack>
        {
            #region NINO_CODEGEN
            public override void Serialize(ConfigAttachToAfterBeAttack value, Nino.Serialization.Writer writer)
            {
                if(value == null)
                {
                    writer.Write(false);
                    return;
                }
                writer.Write(true);
                writer.Write(value.Actions);
            }

            public override ConfigAttachToAfterBeAttack Deserialize(Nino.Serialization.Reader reader)
            {
                if(!reader.ReadBool())
                    return null;
                ConfigAttachToAfterBeAttack value = new ConfigAttachToAfterBeAttack();
                value.Actions = reader.ReadArray<TaoTie.ConfigAbilityAction>();
                return value;
            }
            #endregion
        }
    }
}