using Microsoft.AspNetCore.Mvc;
using NHibernate;
using SisWebCrud.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SisWebCrud.Controllers
{
	public class ClienteController : Controller
	{
		private readonly NHibernate.ISession _session;

		public ClienteController()
		{
			_session = NHibernateHelper.OpenSession(); // Abrindo sessão NHibernate
		}

		// POST: Cliente/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create([Bind("Nome,Tipo,Documento,Endereco,DataCadastro")] Cliente cliente, List<string> Telefones, int TelefoneAtivoId)
		{
			if (ModelState.IsValid)
			{
				cliente.Telefones = new List<Telefone>();

				for (int i = 0; i < Telefones.Count; i++)
				{
					var telefone = new Telefone
					{
						Numero = Telefones[i],
						Ativo = i == TelefoneAtivoId,
						Cliente = cliente // O telefone precisa ter um Cliente associado!
					};
					cliente.Telefones.Add(telefone);
				}

				using (var transaction = _session.BeginTransaction())
				{
					_session.Save(cliente);
					transaction.Commit();
				}

				return RedirectToAction(nameof(Index));
			}

			return View(cliente);
		}


		// GET: Cliente
		public IActionResult Index(string filtroNome, string filtroDocumento)
		{
			var clientesQuery = _session.Query<Cliente>().Where(c => !c.IsDeleted);

			if (!string.IsNullOrEmpty(filtroNome))
			{
				clientesQuery = clientesQuery.Where(c => c.Nome.Contains(filtroNome));
			}

			if (!string.IsNullOrEmpty(filtroDocumento))
			{
				clientesQuery = clientesQuery.Where(c => c.Documento.Contains(filtroDocumento));
			}

			var clientes = clientesQuery.ToList(); // Consulta NHibernate
			//var clientes = _session.Query<Cliente>().ToList();
			foreach (var cliente in clientes)
			{
				Console.WriteLine($"Id: {cliente.Id}, Nome: {cliente.Nome}, Tipo: {cliente.Tipo}");
			}


			// Debug: Verificando os valores de TipoCliente
			foreach (var cliente in clientes)
			{
				Console.WriteLine($"Id: {cliente.Id}, Nome: {cliente.Nome}, Tipo: {cliente.Tipo}");
			}

			return View(clientes);
		}

		// GET: Cliente/Create
		public IActionResult Create()
		{
			var cliente = new Cliente { Telefones = new List<Telefone>(), DataCadastro = DateTime.Now };
			return View(cliente);
		}

		// GET: Cliente/Edit
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var cliente = _session.Query<Cliente>().Where(c => c.Id == id && !c.IsDeleted).FirstOrDefault();

			if (cliente == null)
			{
				return NotFound();
			}

			return View(cliente);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, [Bind("Id,Nome,Tipo,Documento,Endereco,DataCadastro")] Cliente cliente, List<string> Telefones, int TelefoneAtivoId)
		{
			if (id != cliente.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var clienteExistente = _session.Query<Cliente>().Where(c => c.Id == id).FirstOrDefault();
					if (clienteExistente == null)
					{
						return NotFound();
					}

					clienteExistente.Nome = cliente.Nome;
					clienteExistente.Tipo = cliente.Tipo;
					clienteExistente.Documento = cliente.Documento;
					clienteExistente.Endereco = cliente.Endereco;
					clienteExistente.DataCadastro = cliente.DataCadastro;
					clienteExistente.Telefones.Clear();

					for (int i = 0; i < Telefones.Count; i++)
					{
						var telefone = new Telefone
						{
							Numero = Telefones[i],
							Ativo = i == TelefoneAtivoId,
							Cliente = clienteExistente // Corrigindo a referência ao Cliente
						};

						clienteExistente.Telefones.Add(telefone);
					}

					using (var transaction = _session.BeginTransaction())
					{
						_session.Update(clienteExistente);
						transaction.Commit();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Erro ao editar cliente: {ex.Message}");
					throw;
				}

				return RedirectToAction(nameof(Index));
			}

			return View(cliente);
		}


		// GET: Cliente/Delete
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var cliente = _session.Query<Cliente>().Where(m => m.Id == id && !m.IsDeleted).FirstOrDefault();

			if (cliente == null)
			{
				return NotFound();
			}

			return View(cliente);
		}

		// POST: Cliente/Delete
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			var cliente = _session.Get<Cliente>(id);
			if (cliente != null)
			{
				cliente.IsDeleted = true; // Soft delete

				using (var transaction = _session.BeginTransaction())
				{
					_session.Update(cliente);
					transaction.Commit();
				}
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
