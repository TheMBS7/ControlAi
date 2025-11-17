const BASE_URL = 'http://localhost:5189/api/Extratos';


export async function fetchExtratos() {
  const res = await fetch(`${BASE_URL}/Display-Extratos`, {
    cache: 'no-store' // para sempre buscar dados atualizados
  });
  if (!res.ok) {
    throw new Error('Falha ao carregar os extratos');
  }
  return res.json();
}

export async function fetchExtratoId(mesId : number) : Promise<Extrato[]> {
  const res = await fetch(`${BASE_URL}/Display-Extrato/${mesId}`, {
    cache: 'no-store' // para sempre buscar dados atualizados
  });

  if(res.status === 404) {
    return [];
  }

  if (!res.ok) {
    throw new Error('Falha ao carregar os extratos');
  }
  return res.json();
}


export async function createExtrato(extrato: Extrato) {
  const response = await fetch(`${BASE_URL}/Create-Extratos`,{
    method: "POST",
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
            ...extrato,
            data: extrato.data.toISOString()
        })
  });
    if(!response.ok) throw new Error("Erro ao crias extrato");
    return response.json();
}

export async function deleteExtrato(id: number, ExcluirTodasParcelas: boolean) {
  const response = await fetch(`${BASE_URL}/Delete/${id}`,{
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ 
      ExcluirParcelas: ExcluirTodasParcelas 
    }),
  })
  if(!response.ok) throw new Error("Erro ao deletar lançamento");
  return;
}

export async function editExtrato(extrato: Extrato) {
  const responde = await fetch(`${BASE_URL}/Edit/${extrato.id}`,{
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(extrato)
  });
  if(!responde.ok) throw new Error("Erro ao editar Extrato")
}

export async function calcularTotalMes(id: number) {
  const response = await fetch(`${BASE_URL}/Display-Total-Mes/${id}`,{
    cache: "no-store"
  });
  if(!response.ok) throw new Error("Erro ao calcular total mês.");

  return response.json();
  
}

export async function calcularTotalAno(ano: number) : Promise<TotalMes[]> {
  const response = await fetch(`${BASE_URL}/Display-Total/${ano}`,{
    cache: "no-store"
  });
  if(!response.ok) throw new Error("Erro ao calcular total ano.");
  
  return response.json();
}

export async function calcularTotalPessoaMes(id: number) {
  const response = await fetch(`${BASE_URL}/Display-Total-Gasto-Por-Pessoa-No-Mes/${id}`,{
    cache: "no-store"
  });
  if(!response.ok) throw new Error("Erro ao calcular total por pessoa no mês.");

  return response.json();
}