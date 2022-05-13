using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semana08
{
    public class Program
    {
        public static DataClasses1DataContext context = new DataClasses1DataContext();
        static void Main(string[] args)
        {
            //IntroToLINQ();
            //DataSource();
            //Filtering();
            //Ordering();
            //Grouping();
            //Grouping2();
            //Joining();
            //Ejercicio1();
            //Ejercicio2();
            //Ejercicio3();
            //Ejercicio4();
            //Ejercicio5();
            Console.Read();
        }
        static void IntroToLINQ()
        {
            //LAS TRES PARTES DE LINQ QUERY:
            //1. DATA SOURCE
            int[] numeros = { 0, 1, 2, 3, 4, 5, 6 };

            //2. CREACION DEL QUERY
            var numQuery = 
                from num in numeros
                where (num % 2) == 0
                select num;

            //3. EJECUCION DEL QUERY
            foreach (var num in numQuery)
            {
                Console.Write("{0,1} ", num);
            }
        }

        static void DataSource()
        {
            var queryAllCustomers = from cust in context.clientes
                                    select cust;
            var queryCustomersLambda = context.clientes.Select(cust => cust);

            foreach (var cust in queryCustomersLambda)
            {
                Console.Write(cust.NombreCompañia);
            }
        }

        static void Filtering()
        {
            var queryLondonCustomers = from cust in context.clientes
                                       where cust.Ciudad == "Londres"
                                       select cust;
            var queryLondonCustomersLambda = context.clientes.Where(
                cust => cust.Ciudad == "Londres");

            foreach(var cust in queryLondonCustomersLambda)
            {
                Console.Write(cust.NombreContacto);
            }
        }

        static void Ordering()
        {
            var queryLondonCustomers3 = from cust in context.clientes
                                       where cust.Ciudad == "Londres"
                                       orderby cust.NombreCompañia ascending
                                       select cust;
            var queryLondonCustomersLambda3 = context.clientes
                .OrderBy(cust => cust.Ciudad)
                .Where(cust => cust.Ciudad == "Londres");

            foreach (var cust in queryLondonCustomersLambda3)
            {
                Console.WriteLine(cust.NombreCompañia);
            }
        }

        static void Grouping()
        {
            var custQuery = from cust in context.clientes
                            group cust by cust.Ciudad;

            var custQueryLambda = context.clientes
                .GroupBy(cust => cust.Ciudad);

            foreach (var customerGroup in custQueryLambda)
            {
                Console.WriteLine(customerGroup.Key);
                foreach(clientes customer in customerGroup)
                {
                    Console.WriteLine("     {0}", customer.NombreCompañia);
                }
            }
        }

        static void Grouping2()
        {
            var custQuery = from cust in context.clientes
                            group cust by cust.Ciudad into custGroup
                            where custGroup.Count() > 2
                            orderby custGroup.Key
                            select custGroup;

            var custQueryLambda = context.clientes
                .OrderBy(cust => cust.Ciudad)
                .GroupBy(cust => cust.Ciudad)
                .Where(cust => cust.Count() > 2);

            foreach (var cust in custQueryLambda)
            {
                Console.WriteLine(cust.Key);
            }
        }

        static void Joining()
        {
            var innerJoinQuery = from cust in context.clientes
                                 join dist in context.Pedidos on cust.idCliente equals dist.IdCliente
                                 select new { CustomerName = cust.NombreCompañia, DistribuidorName = dist.PaisDestinatario };

            var innerJoinQueryLambda = context.clientes
                .Join(context.Pedidos,
                cust => cust.idCliente,
                dist => dist.IdCliente,
                (cust, dist) => new { CustomerName = cust.NombreCompañia, DistribuidorName = dist.PaisDestinatario });
            
            foreach (var cust in innerJoinQueryLambda)
            {
                Console.WriteLine("CN: "+ cust.CustomerName+ 
                    "           DN: "+ cust.DistribuidorName);
            }
        }

        static void Ejercicio1()
        {

            var query = from clientes in context.clientes
                        join pedidos in context.Pedidos on clientes.idCliente equals pedidos.IdCliente
                        group clientes by clientes.NombreContacto into pedidosGrupo
                        where pedidosGrupo.Key.Count() > 2
                        select pedidosGrupo;

            var queryLambda = context.clientes
                .Join(context.Pedidos,
                    clientes => clientes.idCliente,
                    pedidos => pedidos.IdCliente,
                    (clientes, pedidos) => new { contacto = clientes.NombreContacto, pedidos = pedidos.FechaPedido })
                .GroupBy(clientes => clientes.contacto)
                .Where(clientes => clientes.Key.Count() > 2);

            foreach (var distGroup in queryLambda)
            {
                Console.WriteLine($"Cliente: {distGroup.Key}, pedidos: {distGroup.Count()}");
                
            }
        }

        static void Ejercicio2()
        {
            var query = from detallePedido in context.detallesdepedidos
                        where detallePedido.cantidad * detallePedido.preciounidad > 200
                        select new {idPedido = detallePedido.idpedido, monto = detallePedido.cantidad*detallePedido.preciounidad};

            foreach (var pedido in query)
            {
                Console.WriteLine($"IdPedido: {pedido.idPedido}, Cantidad x Precio: {pedido.monto}");
            }
        }

        static void Ejercicio3()
        {
            var query = from prov in context.proveedores
                        join prod in context.productos on prov.idProveedor equals prod.idProveedor
                        group prov by prov.nombreCompañia into provGroup
                        where provGroup.Count() > 2
                        select provGroup;

            foreach (var provGroup in query)
            {
                Console.WriteLine($"Proveedor: {provGroup.Key}, productos: {provGroup.Count()}");

            }
        }

        static void Ejercicio4()
        {
            var query = from cust in context.clientes
                        group cust by cust.Pais into custGroup
                        where custGroup.Count() > 2
                        select custGroup;

            foreach (var custGroup in query)
            {
                Console.WriteLine($"Pais: {custGroup.Key}, Clientes: {custGroup.Count()}");
                foreach (clientes customer in custGroup)
                {
                    Console.WriteLine("     {0}", customer.NombreContacto);
                }
            }
        }

        static void Ejercicio5()
        {
            var query = from ped in context.Pedidos
                        join comp in context.compañiasdeenvios on ped.FormaEnvio equals comp.idCompañiaEnvios
                        group ped by comp.nombreCompañia into compGroup
                        where compGroup.Count() > 3
                        select compGroup;

            foreach (var compGroup in query)
            {
                Console.WriteLine($"Compañia: {compGroup.Key}, empleados: {compGroup.Count()}");

            }
        }


    }
}
