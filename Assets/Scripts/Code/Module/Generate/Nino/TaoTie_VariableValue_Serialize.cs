/* this is generated by nino */
namespace TaoTie
{
    public partial class VariableValue
    {
        public static VariableValue.SerializationHelper NinoSerializationHelper = new VariableValue.SerializationHelper();
        public class SerializationHelper: Nino.Serialization.NinoWrapperBase<VariableValue>
        {
            #region NINO_CODEGEN
            public override void Serialize(VariableValue value, Nino.Serialization.Writer writer)
            {
                if(value == null)
                {
                    writer.Write(false);
                    return;
                }
                writer.Write(true);
                writer.Write(value.isDynamic);
                writer.Write(value.key);
                writer.CompressAndWrite(ref value.fixedValue);
            }

            public override VariableValue Deserialize(Nino.Serialization.Reader reader)
            {
                if(!reader.ReadBool())
                    return null;
                VariableValue value = new VariableValue();
                reader.Read<System.Boolean>(ref value.isDynamic, 1);
                value.key = reader.ReadString();
                reader.DecompressAndReadNumber<System.Int32>(ref value.fixedValue);
                return value;
            }
            #endregion
        }
    }
}