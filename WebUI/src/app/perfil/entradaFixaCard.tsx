'use client'

import { Card, CardAction, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { useEffect, useState } from "react";
import { criarEntradaFixa, deletarEntradaFixa, editarEntradaFixa, fetchEntradasFixas } from "@/app/services/entradaFixaService";
import { Button } from "@/components/ui/button";
import { Pencil, Trash, ChevronDownIcon } from 'lucide-react'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Input } from "@/components/ui/input";
import { NumericFormat } from 'react-number-format';
import { Popover, PopoverTrigger, PopoverContent } from "@/components/ui/popover";
import { Calendar } from "@/components/ui/calendar"
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { fetchPessoas } from "../services/pessoaService";
import { fetchCategorias } from "../services/categoriaService";

export default function EntradaFixaCard(){
    const [entradasFixas, setEntradasFixas] = useState<EntradaFixa[]>([]);
    const [novaEntradaFixa, setNovaEntradaFixa ] = useState<EntradaFixa | undefined>(undefined);
    const [mostrarForm, setMostrarForm] = useState(false)
    const [open, setOpen] = useState(false)
    const [entradaSendoEditada, setEntradaSendoEditada] = useState<EntradaFixa | undefined>(undefined);
    const [categorias, setCategorias] = useState<Categoria[]>([]);
    const [pessoas, setPessoas] = useState<Pessoa[]>([]);


    useEffect(() => {
        carregarEntradasFixas();
        buscarCategorias();
        buscarPessoas()
    },[])
    

    async function carregarEntradasFixas() {
        try{
            const data = await fetchEntradasFixas();
            setEntradasFixas(data);
        } catch(erro){
            console.error("Erro ao carregar pessoas:", erro);
        }
    }

    async function buscarCategorias() {
            try{
                const categoriasList = await fetchCategorias();
                setCategorias(categoriasList);
            }catch(erro){
                console.error("Erro ao buscar categorias", erro)
            }
        }
    
    async function buscarPessoas() {
        try{
            const pessoaList = await fetchPessoas();
            setPessoas(pessoaList)
        }catch(erro){
            console.error('Erro ao buscar pessoas', erro)
        }
        }

    async function handleCriar() {
        if(!novaEntradaFixa) return;
        
        if(!novaEntradaFixa.descricao.trim()) return;

        try{
            await criarEntradaFixa(novaEntradaFixa);
            setNovaEntradaFixa(undefined);
            setMostrarForm(false);
            await carregarEntradasFixas();
        }catch(erro){
            console.error(erro);
        }
    }

    async function handleDelete(id: number) {
        try{
            await deletarEntradaFixa(id)
            await carregarEntradasFixas();
        }catch(erro){
            console.error(erro)
        }
    }

    async function handleEditar(entradaFixa: EntradaFixa) {
        
        if(!entradaSendoEditada) return;

        try{
            await editarEntradaFixa(entradaFixa);
            await carregarEntradasFixas();

        }catch(erro){
            console.log(erro)
        }
        
    }
    
    return(
        <div>
            <Card className="w-[70%] mx-auto">
                <CardHeader>
                    <CardTitle className=" text-3xl">Entradas Fixas no Mês</CardTitle>
                    <CardAction>
                        <Button
                        variant="ghost"
                        className="bg-[#12698A] font-bold"
                        onClick={() => setMostrarForm(true)}
                        >
                            <p>Nova Entrada</p>    
                        </Button>
                        {mostrarForm && (
                            <>
                                <div className="fixed inset-0 bg-background opacity-60 z-40"></div>
                                <Card className="fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 z-50 p-4 max-w-4xl shadow-lg">
                                    <CardHeader>    
                                        <CardTitle>Cadastro de Nova Entrada</CardTitle>
                                    </CardHeader>
                                    <CardContent className="flex gap-2">
                                        <Input
                                        placeholder="Digite a descrição"
                                        value={novaEntradaFixa?.descricao ?? ""} // valida se a descrição estiver nula, inicializa com string vazia. Precisa disso pois o input não gosta de iniciar undefined e virar string no meio.
                                        onChange={(e => setNovaEntradaFixa((prev) => ({
                                            ...prev!, //aqui matém os outros campos da novaEntradaFixa como estão e atualiza somente o da condição de baixo
                                            descricao: e.target.value //aqui eu escolho qual o campo eu to atualizando nesse input
                                        })))}>
                                        </Input>
                                        <Select
                                        onValueChange={(value) => {
                                            const pessoaSelecionada = pessoas.find(p => p.nome === value)
                                            setNovaEntradaFixa((prev) => ({
                                                ...prev!,
                                                pessoaId: pessoaSelecionada?.id ?? 0
                                            }));
                                        }}>
                                            <SelectTrigger className="w-[25%]">
                                                <SelectValue placeholder="Pessoa"/>
                                            </SelectTrigger>
                                            <SelectContent>
                                                <SelectGroup>
                                                    <SelectLabel className="text-center">Pessoas</SelectLabel>
                                                    {pessoas.map((pessoa) => (
                                                        <SelectItem 
                                                        value={pessoa.nome} 
                                                        key={pessoa.id} 
                                                        >
                                                            {pessoa.nome}
                                                        </SelectItem>
                                                    ))}
                                                </SelectGroup>
                                            </SelectContent>
                                        </Select>
                                        <Select
                                        onValueChange={(value) => {
                                            const categoriaSelecionada = categorias.find(cate => cate.nome === value)
                                            setNovaEntradaFixa((prev) => ({
                                                ...prev!,
                                                categoriaId: categoriaSelecionada?.id ?? 0
                                            }));
                                        }}>
                                            <SelectTrigger className="w-[25%]">
                                                <SelectValue placeholder="Categoria"/>
                                            </SelectTrigger>
                                            <SelectContent>
                                                <SelectGroup>
                                                    <SelectLabel className="text-center">Categorias</SelectLabel>
                                                    {categorias.map((categoria) => (
                                                        <SelectItem 
                                                        value={categoria.nome} 
                                                        key={categoria.id} 
                                                        >
                                                            {categoria.nome}
                                                        </SelectItem>
                                                    ))}
                                                </SelectGroup>
                                            </SelectContent>
                                        </Select>
                                        <NumericFormat
                                        className=""
                                        value={novaEntradaFixa?.valor}
                                        thousandSeparator="."
                                        decimalSeparator=","
                                        prefix="R$ "
                                        decimalScale={2}
                                        allowNegative={false}
                                        customInput={Input}
                                        onValueChange={(values) => {
                                            setNovaEntradaFixa((prev) => ({
                                            ...prev!,
                                             valor: values.floatValue!
                                            }));
                                        }}
                                        />
                                        <Popover open={open} onOpenChange={setOpen}>
                                            <PopoverTrigger asChild>
                                                <Button
                                                variant="ghost"
                                                id="date"
                                                className="w-40 justify-between font-normal"
                                                >
                                                {novaEntradaFixa?.dataReferencia ? novaEntradaFixa.dataReferencia.toLocaleDateString() : "Selecione a Data"}
                                                <ChevronDownIcon/>    
                                                </Button>
                                            </PopoverTrigger>
                                            <PopoverContent className="w-auto overflow-hidden p-0" align="start">
                                                <Calendar
                                                mode="single"
                                                selected={novaEntradaFixa?.dataReferencia}
                                                captionLayout="dropdown"
                                                onSelect={(date) => {
                                                    setNovaEntradaFixa((prev) => ({
                                                        ...prev!,
                                                        dataReferencia: new Date(date!)
                                                    }))
                                                    setOpen(false)
                                                }}
                                                />
                                            </PopoverContent>
                                        </Popover>
                                    </CardContent>
                                    <CardFooter className="gap-2">
                                        <Button
                                        variant="ghost"
                                        className="bg-[#12698A] font-bold"
                                        onClick={() => {
                                            setMostrarForm(false);
                                            setNovaEntradaFixa(undefined);
                                            }}>
                                            Cancelar
                                        </Button>
                                        <Button
                                        variant="ghost"
                                        className="bg-[#12698A] font-bold"
                                        onClick={() => {
                                            handleCriar()
                                        }}
                                        >
                                            Salvar
                                        </Button>
                                    </CardFooter>
                                </Card>
                            </>
                            
                        )}
                    </CardAction>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead></TableHead>
                                <TableHead className="font-bold">DESCRIÇÃO</TableHead>
                                <TableHead className="font-bold">PESSOA</TableHead>
                                <TableHead className="font-bold">CATEGORIA</TableHead>
                                <TableHead className="font-bold">VALOR</TableHead>
                                <TableHead className="font-bold">REFERÊNCIA</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                        {entradasFixas.map((entradaFixa) => {
                            const categoriaMapeada = categorias.find(categoria => categoria.id === entradaFixa.categoriaId);
                            const pessoaMapeada = pessoas.find(pessoa => pessoa.id === entradaFixa.pessoaId);
                            return (
                            <TableRow key={entradaFixa.id}>
                                <TableCell>
                                    <Button
                                    variant="ghost"
                                    onClick={() =>{
                                        console.log(entradaFixa)
                                        setEntradaSendoEditada({...entradaFixa,
                                            dataReferencia: new Date(entradaFixa.dataReferencia)
                                        })
                                        console.log(entradaFixa)
                                    }}
                                    >
                                        <Pencil className="text-blue-500"/>
                                    </Button>
                                   {entradaSendoEditada?.id === entradaFixa.id &&
                                    <>
                                        <div className="fixed inset-0 bg-background opacity-60 z-40"></div>
                                        <Card className="fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 z-50 p-4 max-w-4xl shadow-lg">
                                            <CardHeader>
                                                <CardTitle className='text-2xl'>Edição de Entrada Fixa</CardTitle>
                                            </CardHeader>
                                            <CardContent className="flex gap-2">
                                                <Input
                                                placeholder="Digite a descrição"
                                                value={entradaSendoEditada?.descricao}
                                                onChange={(e => setEntradaSendoEditada((prev) => ({
                                                    ...prev!, //aqui matém os outros campos da novaEntradaFixa como estão e atualiza somente o da condição de baixo
                                                    descricao: e.target.value //aqui eu escolho qual o campo eu to atualizando nesse input
                                                })))}>
                                                </Input>
                                                <Select
                                                value={pessoas.find(p => p.id === entradaSendoEditada.pessoaId)?.nome}
                                                onValueChange={(value) => {
                                                    const pessoaSelecionada = pessoas.find(p => p.nome === value)
                                                    setEntradaSendoEditada((prev) => ({
                                                        ...prev!,
                                                        pessoaId: pessoaSelecionada?.id ?? 0
                                                    }));
                                                }}>
                                                    <SelectTrigger className="w-[25%]">
                                                        <SelectValue placeholder="Pessoa"/>
                                                    </SelectTrigger>
                                                    <SelectContent>
                                                        <SelectGroup>
                                                            <SelectLabel className="text-center">Pessoas</SelectLabel>
                                                            {pessoas.map((pessoa) => (
                                                                <SelectItem 
                                                                value={pessoa.nome} 
                                                                key={pessoa.id} 
                                                                >
                                                                    {pessoa.nome}
                                                                </SelectItem>
                                                            ))}
                                                        </SelectGroup>
                                                    </SelectContent>
                                                </Select>
                                                <Select
                                                value={categorias.find(c => c.id === entradaSendoEditada.categoriaId)?.nome}
                                                onValueChange={(value) => {
                                                    const categoriaSelecionada = categorias.find(cate => cate.nome === value)
                                                    setEntradaSendoEditada((prev) => ({
                                                        ...prev!,
                                                        categoriaId: categoriaSelecionada?.id ?? 0
                                                    }));
                                                }}>
                                                    <SelectTrigger className="w-[25%]">
                                                        <SelectValue placeholder="Categoria"/>
                                                    </SelectTrigger>
                                                    <SelectContent>
                                                        <SelectGroup>
                                                            <SelectLabel className="text-center">Categorias</SelectLabel>
                                                            {categorias.map((categoria) => (
                                                                <SelectItem 
                                                                value={categoria.nome} 
                                                                key={categoria.id} 
                                                                >
                                                                    {categoria.nome}
                                                                </SelectItem>
                                                            ))}
                                                        </SelectGroup>
                                                    </SelectContent>
                                                </Select>
                                                <NumericFormat
                                                className=""
                                                value={entradaSendoEditada?.valor}
                                                thousandSeparator="."
                                                decimalSeparator=","
                                                prefix="R$ "
                                                decimalScale={2}
                                                allowNegative={false}
                                                customInput={Input}
                                                onValueChange={(values) => {
                                                    setEntradaSendoEditada((prev) => ({
                                                    ...prev!,
                                                    valor: values.floatValue!
                                                    }));
                                                }}
                                                />
                                                <Popover open={open} onOpenChange={setOpen}>
                                                    <PopoverTrigger asChild>
                                                        <Button
                                                        variant="ghost"
                                                        id="date"
                                                        className="w-40 justify-between font-normal"
                                                        >
                                                         {entradaSendoEditada?.dataReferencia ? new Date(entradaSendoEditada.dataReferencia).toLocaleDateString() : "Selecione a Data"}
                                                        <ChevronDownIcon/>    
                                                        </Button>
                                                    </PopoverTrigger>
                                                    <PopoverContent className="w-auto overflow-hidden p-0" align="start">
                                                        <Calendar
                                                        mode="single"
                                                        selected={entradaSendoEditada.dataReferencia}
                                                        captionLayout="dropdown"
                                                        onSelect={(date) => {
                                                            setEntradaSendoEditada((prev) => ({
                                                                ...prev!,
                                                                dataReferencia: date!
                                                            }))
                                                            setOpen(false)
                                                        }}
                                                        />
                                                    </PopoverContent>
                                                </Popover>
                                            </CardContent>
                                            <CardFooter className="gap-2">
                                                <Button
                                                variant="ghost"
                                                className="bg-[#12698A] font-bold"
                                                onClick={() => {
                                                    setEntradaSendoEditada(undefined);
                                                    }}>
                                                    Cancelar
                                                </Button>
                                                <Button
                                                variant="ghost"
                                                className="bg-[#12698A] font-bold"
                                                onClick={() => {
                                                    handleEditar(entradaSendoEditada);
                                                    setEntradaSendoEditada(undefined);
                                                }}
                                                >
                                                    Salvar
                                                </Button>
                                            </CardFooter>
                                        </Card>
                                    </>
                                   }
                                    <Button
                                    variant="ghost"
                                    onClick={() => {
                                        handleDelete(entradaFixa.id);
                                    }}
                                    >
                                        <Trash className="text-red-500"/>
                                    </Button>
                                
                                </TableCell>
                                <TableCell>
                                    {entradaFixa.descricao}
                                </TableCell>
                                <TableCell>
                                    {pessoaMapeada?.nome}
                                </TableCell>
                                <TableCell>
                                    {categoriaMapeada?.nome}
                                </TableCell>
                                <TableCell>
                                    {new Intl.NumberFormat("pt-BR", {
                                        style: "currency",
                                        currency: "BRL",
                                    }).format(entradaFixa.valor)}
                                </TableCell>
                                <TableCell>
                                    {new Date(entradaFixa.dataReferencia).toLocaleDateString()}
                                </TableCell>
                            </TableRow>
                            )
                        })}
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>
        </div>
        
    );
}