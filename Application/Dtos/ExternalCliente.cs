namespace Application.Dtos
{
    public class ExternalCliente
    {
        public int idcliente { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? documento { get; set; }
        public string? tipo_documento { get; set; }
        public string? email { get; set; }
        public string? telefono { get; set; }
        public string? direccion { get; set; }
        public string? localidad { get; set; }
        public string? provincia { get; set; }
        public string? codigo_postal { get; set; }
        public DateTime? fecha_alta { get; set; }
    }
}
