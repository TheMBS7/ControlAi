interface EntradaFixa {
    id: number;
    descricao: string;
    valor: number;
    dataReferencia: Date;
    categoriaId: number;
    pessoaId: number;
}

interface SaidaFixa {
    id: number;
    descricao: string;
    valor: number;
    dataVencimento: Date;
    categoriaId: number;
    pessoaId: number;
}

interface Categoria {
    id: number;
    nome: string;
}

interface Extrato {
    id: number;
    descricao: string;
    valorTotal: number;
    data: Date;
    numeroMaxParcelas: number;
    numeroParcela: number;
    categoriaId: number;
    pessoaId: number;
    mesId: number;
    tipoMovimentoId: number;
    
}

interface Pessoa {
    id: number;
    nome: string;
}

interface Mes {
    id: number;
    descricao: string;
    dataInicial: Date;
    dataFinal: Date;
}

interface TipoMovimento {
    id: number,
    tipo: string
}

interface TotalMes {
        mesId: number,
        totalMes: number
    }