import 'bootstrap/dist/css/bootstrap.min.css';

type BigFieldProps = {
    naam: string;
    prijs: number;
    plaatje: string;
    duur: string;
    info: string;
    oogst: string;
    leverancier: string;
    regio: string;
    aantal: number;
};

export default function BigInfoVeld({
                                        naam,
                                        prijs,
                                        plaatje,
                                        duur,
                                        info,
                                        oogst,
                                        leverancier,
                                        regio,
                                        aantal,
                                    }: BigFieldProps) {
    const imageSrc = `/${plaatje}`;

    return (
        <div
            className="d-flex flex-column p-4 w-100 h-100 text-black bg-white"
            style={{
                backgroundColor: 'white',
                border: '5px solid black',
                borderRadius: '10px',
                gap: '10px',
                fontFamily: 'Inter, sans-serif',
                position: 'relative',
            }}
        >
            <img
                src={imageSrc}
                alt={naam}
                className="mx-auto mb-3"
                style={{
                    width: '60%',
                    height: 'auto',
                    backgroundColor: '#3A3A3A',
                    borderRadius: '8px',
                    objectFit: 'cover',
                    border: '2px solid #25632C',
                }}
            />
            <div className="d-flex flex-column gap-1">
                <h2 className="m-0" style={{ fontSize: '24px', fontWeight: 600 }}>{naam}</h2>
                <p className="m-0 text-decoration-line-through" style={{ fontSize: '20px', fontWeight: 500, opacity: 0.8 }}>{prijs}</p>
                <p className="m-0" style={{ fontSize: '18px', fontWeight: 400 }}>{duur}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{info}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{oogst}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{leverancier}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{regio}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{aantal}</p>
            </div>
        </div>
    );
}
