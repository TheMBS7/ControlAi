const BASE_URL = 'http://localhost:5189/api/Meses';

export async function fetchPeriodos(): Promise<Mes[]> {
    const response = await fetch(`${BASE_URL}/Display-Periodos`, {
        cache: "no-store"
    });
    if(!response.ok){
        throw new Error("Falha ao buscar periodos.")
    }
    return response.json();
}

export async function fetchPeriodosId(id: number): Promise<Mes> {
    const response = await fetch(`${BASE_URL}/Display-Periodo/${id}`, {
        cache: "no-store"
    });
    if(!response.ok){
        throw new Error("Falha ao buscar periodo.")
    }
    return response.json();
}

export async function novoPeriodo() {
    const response = await fetch(`${BASE_URL}/Criar-Periodos`,{
        method: "POST"
    });
    if(!response.ok){
        throw new Error("Falha ao criar periodo.")
    }
    return;
    
}