'use client'

import { Button } from "@/components/ui/button";
import { Card, CardAction, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { useEffect, useState } from "react";
import { createSaidaFixa, deleteSaidaFixa, editarSaidaFixa, fetchSaidasFixas } from "../services/saidaFixaService";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { ChevronDownIcon, Pencil, Trash } from "lucide-react";
import { fetchCategorias } from "../services/categoriaService";
import { Input } from "@/components/ui/input";
import { NumericFormat } from "react-number-format";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { Calendar } from "@/components/ui/calendar";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";



export default function  SaidaFixaCard(){
    const [saidasFixas, setSaidasFixas] = useState<SaidaFixa[]>([]);
    const [categorias, setCategorias] = useState<Categoria[]>([]);
    const [mostrarForm, setMostrarForm] = useState(false);
    const [novaSaidaFixa, setNovaSaidaFixa] = useState<SaidaFixa | undefined>(undefined);
    const [open, setOpen] = useState(false)
    const [saidaSendoEditada, setSaidaSendoEditada] = useState<SaidaFixa | undefined>(undefined);

    useEffect(() => {
        buscarSaidasFixas();
        buscarCategorias();
    },[])

    async function buscarSaidasFixas(){
        try{
            const data = await fetchSaidasFixas()
            setSaidasFixas(data);
        }catch(erro){
            console.error("Erro ao carregar Saidas Fixas", erro)
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

    async function handleCriar() {
        
        if (!novaSaidaFixa) return;
        
        if(!novaSaidaFixa.descricao.trim()) return;
        novaSaidaFixa.pessoaId = 72;
        try{
            await createSaidaFixa(novaSaidaFixa);
            setNovaSaidaFixa(undefined);
            setMostrarForm(false);
            await buscarSaidasFixas();
            
        }catch(erro){
            console.log("erro ao criar saída fixa", erro)
        }
    }

    async function handleDelete(id: number) {
        try{
            await deleteSaidaFixa(id);
            await buscarSaidasFixas();
        }catch(erro){
            console.log("erro ao deletar Saída Fixa", erro)
        }
    }

    async function handleEditar(saidaFixa: SaidaFixa) {

        try{
            await editarSaidaFixa(saidaFixa);
            await buscarSaidasFixas();
        }catch(erro){
            console.log("erro ao editar saída fixa", erro)
        }
    }

    // console.log("AQUI CATEGORIAAAA",categorias)
    return(
        <div>
            <Card className="w-[80%] mx-auto">
                <CardHeader>
                    <CardTitle className="text-3xl">Saídas Fixas no Mês</CardTitle>
                    <CardAction>
                        <Button
                        className="bg-[#12698A] font-bold"
                        variant="ghost"
                        onClick={() => setMostrarForm(true)}
                        >
                            Nova Saída
                        </Button>
                        {mostrarForm && (
                            <>
                                <div className="fixed inset-0 bg-background opacity-60 z-40"></div>
                                <Card className="fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 z-50 p-4  max-w-3xl shadow-lg">
                                    <CardHeader>
                                        <CardTitle>Criação de Nova Saída</CardTitle>
                                    </CardHeader>
                                    <CardContent className="flex gap-2">
                                        <Input
                                        placeholder="Digite a Descrição"
                                        onChange={e => setNovaSaidaFixa((prev) => ({
                                            ...prev!,
                                            descricao: e.target.value
                                        }))}
                                        />
                                        <Select
                                        
                                        onValueChange={(value) => {
                                            const categoriaSelecionada = categorias.find(cate => cate.nome === value)
                                            setNovaSaidaFixa((prev) => ({
                                                ...prev!,
                                                categoriaId: categoriaSelecionada?.id ?? 0
                                            }));
                                        }}>
                                            <SelectTrigger className="w-[25%]">
                                                <SelectValue placeholder="Selecione a Categoria"/>
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
                                        value={novaSaidaFixa?.valor ?? 0}
                                        thousandSeparator="."
                                        decimalSeparator=","
                                        prefix="R$ "
                                        decimalScale={2}
                                        allowNegative={false}
                                        customInput={Input}
                                        onValueChange={(values) => {
                                            setNovaSaidaFixa((prev) => ({
                                                ...prev!,
                                                valor: values.floatValue ?? 0
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
                                                {novaSaidaFixa?.dataVencimento ? novaSaidaFixa.dataVencimento.toLocaleDateString() : "Selecione a Data"}
                                                <ChevronDownIcon/>    
                                                </Button>
                                            </PopoverTrigger>
                                            <PopoverContent className="w-auto overflow-hidden p-0" align="start">
                                                <Calendar
                                                mode="single"
                                                selected={novaSaidaFixa?.dataVencimento}
                                                captionLayout="dropdown"
                                                onSelect={(date) => {
                                                    setNovaSaidaFixa((prev) => ({
                                                        ...prev!,
                                                        dataVencimento: new Date(date!)
                                                    }))
                                                    setOpen(false)
                                                }}
                                                />
                                            </PopoverContent>
                                        </Popover>
                                    </CardContent>
                                    <CardFooter className="gap-2">
                                        <Button
                                        className="bg-[#12698A] font-bold"
                                        variant="ghost"
                                        onClick={() => {
                                            setMostrarForm(false);
                                            setNovaSaidaFixa(undefined);
                                        }}
                                        >
                                            Cancelar
                                        </Button>
                                        <Button
                                        className="bg-[#12698A] font-bold"
                                        variant='ghost'
                                        onClick={() => {
                                            
                                            handleCriar();
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
                                <TableHead/>
                                <TableHead className="font-bold">DESCRIÇÃO</TableHead>
                                <TableHead className="font-bold">CATEGORIA</TableHead>
                                <TableHead className="font-bold">VALOR</TableHead>
                                <TableHead className="font-bold">VENCIMENTO</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                        {saidasFixas.map((saidaFixa) => {
                            const categoriaMapeada = categorias.find(categoria => categoria.id === saidaFixa.categoriaId);
                            return (
                            <TableRow key={saidaFixa.id}>
                                <TableCell>
                                    <Button
                                    variant="ghost"
                                    onClick={() => {
                                        setSaidaSendoEditada(saidaFixa)

                                    }}
                                    >
                                        <Pencil className="text-blue-500"/>
                                    </Button>
                                    
                                    {saidaSendoEditada?.id === saidaFixa.id &&
                                    <>
                                        <div className="fixed inset-0 bg-background opacity-60 z-40"></div>
                                        <Card className="fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 z-50 p-4  max-w-3xl shadow-lg">
                                            <CardHeader>
                                                <CardTitle className='text-2xl'>Edição de Saída Fixa</CardTitle>
                                            </CardHeader>
                                            <CardContent className="flex gap-2">
                                                <Input
                                                placeholder="Digite a Descrição"
                                                value={saidaSendoEditada.descricao}
                                                onChange={e => setSaidaSendoEditada((prev) => ({
                                                    ...prev!,
                                                    descricao: e.target.value
                                                }))}
                                                />
                                                <Select
                                                value={categorias.find(c => c.id === saidaSendoEditada.categoriaId)?.nome}
                                                onValueChange={(value) => {
                                                    const categoriaSelecionada = categorias.find(cate => cate.nome === value)
                                                    setSaidaSendoEditada((prev) => ({
                                                        ...prev!,
                                                        categoriaId: categoriaSelecionada?.id ?? 0
                                                    }));
                                                }}>
                                                    <SelectTrigger className="w-[25%]">
                                                        <SelectValue placeholder="Selecione a Categoria"/>
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
                                                value={saidaSendoEditada?.valor}
                                                thousandSeparator="."
                                                decimalSeparator=","
                                                prefix="R$ "
                                                decimalScale={2}
                                                allowNegative={false}
                                                customInput={Input}
                                                onValueChange={(values) => {
                                                    setSaidaSendoEditada((prev) => ({
                                                        ...prev!,
                                                        valor: values.floatValue ?? 0
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
                                                        {saidaSendoEditada?.dataVencimento ? new Date(saidaSendoEditada.dataVencimento).toLocaleDateString() : "Selecione a Data"}
                                                        <ChevronDownIcon/>    
                                                        </Button>
                                                    </PopoverTrigger>
                                                    <PopoverContent className="w-auto overflow-hidden p-0" align="start">
                                                        <Calendar
                                                        mode="single"
                                                        selected={saidaSendoEditada?.dataVencimento}
                                                        captionLayout="dropdown"
                                                        onSelect={(date) => {
                                                            setSaidaSendoEditada((prev) => ({
                                                                ...prev!,
                                                                dataVencimento: new Date(date!)
                                                            }))
                                                            setOpen(false)
                                                        }}
                                                        />
                                                    </PopoverContent>
                                                </Popover>
                                            </CardContent>
                                            <CardFooter className="gap-2">
                                                <Button
                                                className="bg-[#12698A] font-bold"
                                                variant="ghost"
                                                onClick={() => {
                                                    setSaidaSendoEditada(undefined);
                                                }}
                                                >
                                                    Cancelar
                                                </Button>
                                                <Button
                                                className="bg-[#12698A] font-bold"
                                                variant='ghost'
                                                onClick={() => {
                                                    handleEditar(saidaSendoEditada);
                                                    setSaidaSendoEditada(undefined);
                                                    
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
                                    onClick={() => handleDelete(saidaFixa.id)}
                                    >
                                        <Trash className="text-red-500"/>
                                    </Button>
                                </TableCell>
                                <TableCell>
                                    {saidaFixa.descricao}
                                </TableCell>
                                <TableCell>
                                    {categoriaMapeada?.nome}
                                </TableCell>
                                <TableCell>
                                    {new Intl.NumberFormat("pt-BR", {
                                        style: "currency",
                                        currency: "BRL",
                                    }).format(saidaFixa.valor)}
                                </TableCell>
                                <TableCell>
                                    {new Date(saidaFixa.dataVencimento).toLocaleDateString()}
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