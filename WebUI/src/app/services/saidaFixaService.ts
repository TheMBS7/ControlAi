const BASE_URL = 'http://localhost:5189/api/SaidasFixas';

export async function fetchSaidasFixas():Promise<SaidaFixa[]>{
    const response = await fetch(`${BASE_URL}/Display-SaidasFixas`,{
        cache: "no-store"
    });
    if (!response.ok) throw new Error("Erro ao buscar Saidas Fixas")
    
    return response.json() as Promise<SaidaFixa[]>;
}

export async function createSaidaFixa(saidaFixa: SaidaFixa) {
    const response = await fetch(`${BASE_URL}/Create-SaidasFixas`,{
        method: "POST",
        headers:{
            "Content-Type" : "application/json"
        },
        body: JSON.stringify({
            ...saidaFixa,
            dataVencimento: saidaFixa.dataVencimento.toISOString()
        })
    });
    if(!response.ok) throw new Error("Erro ao criar saida fixa");

    return response.json();
}

export async function deleteSaidaFixa(id: number) {
    await fetch(`${BASE_URL}/Delete/${id}`,{
        method: "DELETE"
    });
    return
}

export async function editarSaidaFixa(saidaFixa: SaidaFixa) {
    const response = await fetch(`${BASE_URL}/Edit/${saidaFixa.id}`,{
        method: "PUT",
        headers: {
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(saidaFixa)
    });
    if (!response.ok) throw new Error("Erro ao editar Saida Fixa");

    return response.json();
}