import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'; // Zorg dat Bootstrap is geïmporteerd
import { parsePrice } from '@/types/Product';
import s from './veilingKlok.module.css';

interface AuctionTimerProps {
    onFinished?: () => void; // Optionele callback als de tijd op is
    startPrice: number; // Beginprijs
    minPrice: number; // Minimumprijs waarop de klok stopt
}

const AuctionTimer: React.FC<AuctionTimerProps> = ({
                                                       // duration = 90,
                                                       onFinished,
                                                       startPrice,
                                                       minPrice
                                                   }) => {
    // Start met de startprijs
    const [currentPrice, setCurrentPrice] = useState<number>(startPrice);

    // Helper om seconden als mm:ss te tonen
    const formatTime = (totalSeconds: number) => {
        const s = Math.max(0, Math.floor(totalSeconds));
        const m = Math.floor(s / 60);
        const rest = s % 60;
        return `${m}:${rest.toString().padStart(2, '0')}`;
    };

    useEffect(() => {
        // Als startPrice al op of onder minPrice is, zet direct naar minPrice
        if (startPrice <= minPrice) {
            setCurrentPrice(minPrice);
            if (onFinished) onFinished();
            return;
        }

        setCurrentPrice(startPrice);

        // Update elke 100ms voor vloeiende animatie
        const intervalTime = 100; // ms
        const decreasePerSecond = startPrice * 0.05; // 5% van startprijs per seconde
        const decreasePerMs = decreasePerSecond / 1000; // per ms

        const timer = setInterval(() => {
            setCurrentPrice((prev) => {
                const newPrice = prev - decreasePerMs * intervalTime;
                if (newPrice <= minPrice) {
                    clearInterval(timer);
                    if (onFinished) onFinished();
                    return minPrice;
                }
                return newPrice;
            });
        }, intervalTime);

        return () => clearInterval(timer);
    }, [startPrice, minPrice, onFinished]);

    // Berekening van het percentage tussen startPrice (100%) en minPrice (0%)
    let percentage = 0;
    if (startPrice > minPrice) {
        percentage = ((currentPrice - minPrice) / (startPrice - minPrice)) * 100;
        // clamp
        percentage = Math.max(0, Math.min(100, percentage));
    }

    // Bereken resterende seconden 1x zodat we het kunnen tonen op een overlay
    const decreasePerSecond = startPrice * 0.05; // zelfde als timer-logica
    const remainingSeconds =
        startPrice > minPrice && decreasePerSecond > 0
            ? (currentPrice - minPrice) / decreasePerSecond
            : 0;


    return (
        <div className="container mt-4">
            <h2>{parsePrice(Math.ceil(currentPrice))} </h2>

            {/* Bootstrap Progress Bar Container */}
            <div className="progress" style={{ height: '30px', position: 'relative' }}>
                <div
                    className={`progress-bar progress-bar progress-bar-animated ${s.balkAnimatie}`}
                    role="progressbar"
                    style={{
                        width: `${percentage}%`,
                    }}
                    aria-valuenow={percentage}
                    aria-valuemin={0}
                    aria-valuemax={100}
                >
                    {/* Leeg laten zodat de tekst niet meeschuift met de voortgang */}
                </div>
                {/* Overlay die altijd gecentreerd in het midden staat */}
                <div className={s.balkTekst}
                    aria-hidden
                >
                    {formatTime(remainingSeconds)}
                </div>
            </div>

            <div className="mt-2 text-muted">
                {currentPrice <= minPrice && "De veiling is gesloten!"}
            </div>
        </div>
    );
};

export default AuctionTimer;