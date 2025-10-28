const BASE_URL = 'http://localhost:5189/api/EntradasFixas';

export async function fetchEntradasFixas():Promise<EntradaFixa[]> {
    const res = await fetch(`${BASE_URL}/Display-EntradasFixas`, {
        cache: "no-store"
    
    });
    if (!res.ok) throw new Error("Erro ao buscar Entradas Fixas");

    return res.json() as Promise<EntradaFixa[]>;
}

export async function criarEntradaFixa(entradaFixa: EntradaFixa) {
    const response = await fetch(`${BASE_URL}/Create-EntradasFixas`,{
        method: 'POST',
        headers:{
            'Content-Type': 'application/json'
        },
       body: JSON.stringify({
            ...entradaFixa,
            dataReferencia: entradaFixa.dataReferencia.toISOString()
        })
    });
    if (!response.ok) throw new Error("Erro ao criar Entrada Fixa");

    return response.json();

}

export async function editarEntradaFixa(entradaFixa: EntradaFixa) {
    const response = await fetch(`${BASE_URL}/Edit/${entradaFixa.id}`,{
        method: 'PUT',
        headers:{
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(entradaFixa)
    });
    if (!response.ok) throw new Error("Erro ao editar Entrada Fixa");

    return response.json();

}

export async function deletarEntradaFixa(id: number) {
    const res = await fetch(`${BASE_URL}/Delete/${id}`,{
        method: 'DELETE',
        headers:{
            'Content-Type': 'application/json'
        }
    });
    if(!res.ok) throw new Error("Erro ao deletar Entrada Fixa");

    return res.json();
}