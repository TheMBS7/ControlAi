const BASE_URL = 'http://localhost:5189/api/Categorias';

export async function fetchCategorias() {
  const res = await fetch(`${BASE_URL}/Display-Categorias`, {
    cache: 'no-store'
  });

  if (!res.ok) {
    const errorText = await res.text(); // loga o que o servidor mandou
    console.error("Backend error:", errorText);
    throw new Error('Erro ao buscar categorias');
  }

  return res.json();
}

export async function criarCategoria(nome: string) {
  const res = await fetch(`${BASE_URL}/Create-Categoria`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ nome })
  });

  if (!res.ok) throw new Error('Erro ao criar categoria');

  return res.json();
}

export async function deleteCategoria(id: number) {
  const res = await fetch(`${BASE_URL}/Delete/${id}`, {
    method: 'DELETE'
  });

  if (!res.ok) {
    // throw new Error('Erro ao deletar categoria');
    const msgErro = await res.text();
    alert(msgErro);
    return;
  }

  return true; // se a API retornar algo
}

export async function editarCategoria(id: number, nome: string) {
  const res = await fetch(`${BASE_URL}/Edit/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ nome })
  });

  if (!res.ok) {
    throw new Error("Erro ao editar categoria");
  }

  return await res.json();
}
